#Live Performance Tracker
# Tracks Sihlouettes and sends their motion data to Unity3d via UDP
# Alpha 0.011
#
#Author: Taylor Brockhoeft
#
#branch: tracker_branch

import sys
sys.path.append('C:\OpenCV_2.4.9\opencv\sources\samples\python2')
from common import nothing, clock, draw_str
from collections import deque
import numpy as np
import socket
import time
import cv2

class Form():
    #Basic Form Class
    # Each form is serializable with self.id
    # 
    # Accessable Datas:
    #   Radius
    #   Velocity
    #   Center
    #   Motion History Length (center)
    #   Convex Hull
    #   Left/Right most points
    #   Direction
    #   lost (used for tracking purposes)
    # Sends UDP Data
    #
    #
    def __init__ (self,id,contour):
        self.id = id
        self.setForm(contour)
        self.HISTORY_LENGTH = 10
        self.history = deque(maxlen=self.HISTORY_LENGTH) #create history
        self.history.appendleft(self.center()) #append initial center
        self.velocity = 0.0
        self.radius = 0
        self.perimeter = 0
        self.rect = cv2.boundingRect(self.form)
        self.convex_hull = cv2.convexHull(self.form) 
        self.leftmost = tuple(self.form[self.form[:,:,0].argmin()][0])
        self.rightmost = tuple(self.form[self.form[:,:,0].argmax()][0])
        self.dirx = 0.0
        self.diry = 0.0
        self.lost = False
        #For Editing Mode, this needs to keep the y value

    def findLeftRightMost(self):
        self.leftmost = tuple(self.form[self.form[:,:,0].argmin()][0])
        self.rightmost = tuple(self.form[self.form[:,:,0].argmax()][0])

    def showLeftRightMost(self):
        x,y = self.center()

        y += 20

        #Performance Mode, set center y as left and rightmost y
        if self.leftmost[1] > y:
            cv2.circle(frame, (self.leftmost[0],y), 5, (0,0,255), 3)
            draw_str(frame, (self.leftmost[0],y), str("left"))
        else:
            cv2.circle(frame, self.leftmost, 5, (0,0,255), 3)
            draw_str(frame, self.leftmost, str("left"))

        if self.rightmost[1] > y:
            cv2.circle(frame, (self.rightmost[0],y), 5, (0,0,255), 3)
            draw_str(frame, (self.rightmost[0],y), str("right"))
        else:
            cv2.circle(frame, self.rightmost, 5, (0,0,255), 3)
            draw_str(frame, self.rightmost, str("right"))
        

    def showConvexHull(self):
        self.getConvexHull()
        for i,c in enumerate(self.convex_hull):
            cv2.circle(frame, (c[0][0],c[0][1]), 5, (255,0,0), 3)

    def getConvexHull(self):
        #Gets Perimeter points        
        self.convex_hull = cv2.convexHull(self.form)        

    def getID (self):
        return self.id

    def setID (self, i):
        self.id = i

    def setForm(self, f):
        self.form = f

    def setHistoryLength(self,l):
        self.HISTORY_LENGTH = l

    def center(self):
        M = cv2.moments(self.form)
        if M["m00"] == 0.0:
            M["m00"] = M["m00"] +1
        return (int(M["m10"] / M["m00"]), int(M["m01"] / M["m00"]))

    def getRadius(self):
        return self.radius

    def update(self):
        self.findLeftRightMost()
        self.history.appendleft(self.center()) #append initial center
        self.calculateVelocity()
        self.calculateDirection()
        self.calculateRadius()
        self.perimeter = cv2.arcLength(self.form, True)
        self.sendUDP();
        self.draw()

    def isInBoundingBox(self, formobj, thresh):
        # Looks for formobj (which is another form) to see if it is within the bounding box + thresh of the self. If so, it is
        self.rect = cv2.boundingRect(self.form)
        x,y,w,h = self.rect
        fx,fy = formobj.center()

        if fx > x and fx < x+w:
            if fy > y and fy < y+h:
                if self.id != formobj.id:
                    print ""
                    #print "Im inside of you"

    def draw(self):
        #self.showConvexHull()
        self.showLeftRightMost()
        drawContour(self.id,(0,255,0),self.form, self.diry)

    def calculateRadius(self):
        '''Uses the bounding box to calculate pixel radius'''
        self.rect = cv2.boundingRect(self.form)
        x,y,w,h = self.rect
        self.radius = min(w/2, h/2)

    def calculateVelocity(self):
        '''calculates average velocity across the HISTORY_LENGTH'''
        if len(self.history) < self.HISTORY_LENGTH and len(self.history) > 1:
            #If less than history length
            startx,starty = self.history[0]
            endx,endy = self.history[len(self.history)-1]
            time = len(self.history)-1
        elif len(self.history) >= self.HISTORY_LENGTH:
            startx,starty = self.history[0]
            endx,endy = self.history[self.HISTORY_LENGTH-1]
            time = self.HISTORY_LENGTH-1
        else:
            startx,starty = 0,0
            endx,endy = 0.0
            time = 1.0

        dist = distance((startx,starty),(endx,endy))
        self.velocity = dist/time

    def sendUDP(self):
        '''message with id at begining'''
        x,y = self.center()
        lx,ly = self.leftmost
        rx,ry = self.rightmost
        mess = "DATA," + str(self.id) + "," + str(x) + "," + str(y) + ","\
                                            + str(lx) + "," + str(ly) + ","\
                                            + str(rx) + "," + str(ry) + ","\
                                            + str(self.radius) + "," + str(self.velocity)
        sock.sendto(mess , (UDP_IP, UDP_PORT))
        #print "sent " ,mess," to ",UDP_PORT, "For", self.id

    def calculateDirection(self):
        '''calculates the direction of a moving form in the x axis based on data stored in the history queue
            larger shifts in movement will result in self.dirx to be a larger value.
        '''
        if len(self.history) < self.HISTORY_LENGTH and len(self.history) > 1:
            #If less than history length
            startx,starty = self.history[0]
            endx,endy = self.history[len(self.history)-1]
        elif len(self.history) >= self.HISTORY_LENGTH:
            startx,starty = self.history[0]
            endx,endy = self.history[self.HISTORY_LENGTH-1]
        else:
            startx,starty = 0,0
            endx,endy = 0.0

        self.dirx =endx-startx
        #print self.dirx

        # if self.dirx > 0:
        #     print self.id,"RIGHT"
        # else:
        #     print self.id,"LEFT"

def drawContour(label,color,contour, r=0):
    '''Draws Contour (C) at center point with color 'c' and label 'l'''
    M = cv2.moments(contour)
    if M["m00"] == 0.0:
        M["m00"] = M["m00"] +1
    center = (int(M["m10"] / M["m00"]), int(M["m01"] / M["m00"]))
    x,y = center
    cv2.circle(frame, center, 5, color, 3)
    draw_str(frame, center, str(label))
    draw_str(frame, (x-30,y), str(r))

def findIndex(cnt,contour):
    '''returns the index of an item in a python array
        IF NOT FOUND then return -1'''
    m1 = cv2.moments(cnt)
    for i,c in enumerate(contour):
        m2 = cv2.moments(c)
        if  m1 == m2:
            return i
    
    return -1

def distance(center,other):
    '''calculates distance between two centers'''
    x,y = center
    a,b = other
    return ((x-a)**2 + (y-b)**2)**0.5 #might be able to drop the exponents later if we need to increse speed


def findNForms(frame,n):
    '''Search for n forms within frame'''
    forms = list()
    cnts,heir = cv2.findContours(frame.copy(), cv2.RETR_EXTERNAL,cv2.CHAIN_APPROX_SIMPLE)

    i = n

    while( i > 0):
        if len(cnts) > 0:       
            largest = max(cnts, key=cv2.contourArea)
            index = findIndex(largest,cnts)#find and remove
            if not index < 0:
                del cnts[index]
                forms.append(largest)
        i -= 1
    return forms

def findFormsDynamic(frame,minArea):
    '''Dynamically Locate and transmit coordinates for all forms'''
    forms = list()
    cnts,heir = cv2.findContours(frame.copy(), cv2.RETR_EXTERNAL,cv2.CHAIN_APPROX_SIMPLE)

    for c in cnts:
        if cv2.contourArea(c) > minArea**2:
            forms.append(c)

    return forms

def initialize():
    '''Initialize all form and meta data and send to Unity'''
    global is_initialized
    is_initialized = True
    sendSettingsUDP("COUNT",formcount) #Sends number of forms tracked to unity
    contours = findNForms(thresh,formcount)
    for i,C in enumerate(contours):
        bodies.append(Form(i,C))
    print "Initialized"

def clear():
    global is_initialized
    global bodies
    is_initialized = False
    bodies = list()
    print "Cleared initialization"

def sendSettingsUDP(name,val):
    ''' Sends Settings in the form "SET, NAME, VAL"'''
    mess = "SET," + str(name) + "," + str(val) + "," 
    sock.sendto(mess , (UDP_IP, UDP_PORT))

def isMerged(a,b):
    '''checks to see if two forms entities have merged
    defined as having different ID's in approximatley the same space.
    '''
    if a.getID() != b.getID():
        if distance(a.center(),b.center()) < 10: #will be zero if they're the same form
            return True

def track():
    tmpforms = list()        
    contours = findNForms(thresh,formcount)

    for i,C in enumerate(contours):
        tmpforms.append(Form(i,C))
        drawContour(i,(255,0,0),C)

    for t in tmpforms:
        t.lost = True

    for b in bodies:
        #Isolate and update Bodies
        minDist = 100 
        closest = None
        for t in tmpforms:
            #newDist = distance(center(t.form),center(b.form))            
            newDist = distance(t.center(),b.center())            

            if newDist < minDist:
                #print "Dist",newDist
                if t.lost == True:
                    minDist = newDist
                    closest = t
                    #print "closest" , closest.id
                    t.lost = False

        if closest:
            #print "Dist", t.id, b.id, center(t.form), center(b.form), newDist
            #print "Set", closest.id ,"to", b.id
            b.setForm(closest.form)
            b.lost = False
            #print "\n"
        else:
            #If none closest, then this body is lost
            #print "Im lost"
            b.lost = True

    for b in bodies:
        b.update()

    # for b in bodies:
    #     for bn in bodies:
    #         b.isInBoundingBox(bn,10)

###GUI
def set_scale_thresh_u(val):
    '''Sets Upperbound Threshold'''
    global thresh_u
    thresh_u = val
def set_scale_thresh_l(val):
    ''' Sets Lowerbound Threshhold'''
    global thresh_l
    thresh_l = val
def set_scale_brightness(val):
    ''' Sets Image Brightness Gain (Up only)'''
    global brightness
    brightness = val
def set_gui_initialized(val):
    ''' GUI set initialized'''
    global is_initialized
    if val == 0:
        clear()
    if val == 1:
        initialize()
def set_gui_formcount(val):
    ''' GUI set bodycount and re-initialize'''
    global formcount
    formcount = val
    clear()
    initialize()
    
def set_gui_exit(val):
    ''' GUI Quit option '''
    if val == 1:
        cv2.destroyAllWindows()
        sys.exit()

cv2.namedWindow('control panel', 0)
cv2.createTrackbar('is_initilized', 'control panel', 0, 1 , set_gui_initialized)
cv2.createTrackbar('formcount', 'control panel', 1, 9 , set_gui_formcount)
cv2.createTrackbar('thresh_lower', 'control panel', 0, 255, set_scale_thresh_l)
cv2.createTrackbar('thresh_upper', 'control panel', 255, 255, set_scale_thresh_u)
cv2.createTrackbar('src brightness', 'control panel', 36, 255 , set_scale_brightness)
cv2.createTrackbar('Exit', 'control panel', 0, 1 , set_gui_exit)
### END GUI

#Consts
UDP_IP = "127.0.0.1"
UDP_PORT = 5005
KEY_I = 105
KEY_C = 99
KEY_ESC = 27
WAIT = 1
#Variables
is_initialized = False
cap = cv2.VideoCapture(0)
fname = "02_100.MP4"
flength = 100 # Length of video file in frames
loop = False
#cap = cv2.VideoCapture(fname)
c = 0 
formcount = 1 #Number of Forms to Expect (can be set through gui)
bodies = list() #Bodies Output
run = True
#GUI Defaults
thresh_l = 45
thresh_u = 255
brightness = 36

print "Setting up"
print "UDP target IP:", UDP_IP
print "UDP target port:", UDP_PORT
sock = socket.socket(socket.AF_INET, # Internet
                 socket.SOCK_DGRAM) # UDP

ret, frame = cap.read()


while(run):
    c+=1
    #print c
    #Set loop to true to enable a looped video for debuging, make sure you have the right length c
    if loop:
        time.sleep(.3)
        if c == 100:
            initialize()
        if c == flength:
            #break
            clear()
            cap = cv2.VideoCapture(fname)
            c = 0

    # Capture frame-by-frame
    ret, frame = cap.read()
    #print ret

    # Our operations on the frame come here
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    gray = gray + brightness
    ret,thresh = cv2.threshold(gray,thresh_l,thresh_u,cv2.THRESH_BINARY)

    if is_initialized:
        track()

    # Display the resulting frame
    cv2.imshow('frame',frame)
    cv2.imshow('thresh',thresh)      

    ##Keyboard shortcuts for debug
    if  (0xFF & cv2.waitKey(WAIT) == KEY_I) and is_initialized == False: #I - initialize (only initialize if we haven't already)
        initialize()

    if (0xFF &  cv2.waitKey(WAIT) == KEY_C) and is_initialized == True: #C - Clear initialization (only if it's been initialized)
        clear()
    if 0xFF & cv2.waitKey(WAIT) == KEY_ESC:
        run = False

cv2.destroyAllWindows()
sys.exit()
