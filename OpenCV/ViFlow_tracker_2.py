#Live Performance Tracker
#Tracks Sihlouettes and sends their motion data to Unity3d via UDP
#
#Author: Taylor Brockhoeft

#Edit 12/04/2016: Rathna Ramesh
#       discrete points to indicate legs and head added
#       comments added to enhance reability



import sys
sys.path.append('C:\OpenCV_2.4.9\opencv\sources\samples\python2')
from common import nothing, clock, draw_str
from collections import deque
import numpy as np
import socket
import time
import array
import cv2
import msgpack
import array
#import msgpack_numpy as mn
#mn.patch()

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
    #   top, bottom (leg 1) and legs
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
        self.extTop = tuple(self.form[self.form[:, :, 1].argmin()][0])
        self.extBot = tuple(self.form[self.form[:, :, 1].argmax()][0])
        self.otherLeg = self.extBot
        self.dirx = 0.0
        self.diry = 0.0
        self.lost = False
        #For Editing Mode, this needs to keep the y value

    def findLeftRightMost(self): #sets the leftmost and the right most pixels - adding head and legs to it
        self.leftmost = tuple(self.form[self.form[:,:,0].argmin()][0])
        self.rightmost = tuple(self.form[self.form[:,:,0].argmax()][0])
        new = 255 - self.form
        self.extTop = tuple(self.form[self.form[:, :, 1].argmin()][0])
        self.extBot = tuple(self.form[self.form[:, :, 1].argmax()][0])
        x, y = self.extTop
        ax, ay = self.extBot
        min_legs = distance(self.extBot, self.rightmost)
        approx = cv2.approxPolyDP(self.form, 0.04 * self.perimeter, True)#find the vertices of the polygon that approximates the contour
        ls = tuple(approx[:,0]) #approx is a 2D matrix of points - just need the first column
        for p in ls:
            t = tuple(p)
            cv2.circle(thresh, t, 8, (127, 255, 0), -1,8,0)
            xi,yi = p
            if (distance((0,yi),(0,ay)) < min_legs) & ( t!=(ax,ay)): #leg - least far from ground + not the bottommost point (the other leg)
                self.otherLeg = t
                min_legs = distance((0,yi),(0,ay))
                
        
        

    def showLeftRightMost(self): #despite the actual y of hands, center y is taken 
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

        cv2.putText(thresh, "head", self.extTop, cv2.FONT_HERSHEY_SIMPLEX,
		0.5, (127, 255, 0), 2)
        cv2.putText(thresh, "leg", self.otherLeg, cv2.FONT_HERSHEY_SIMPLEX,
		0.5, (127, 255, 0), 2)
        cv2.putText(thresh, "leg", self.extBot, cv2.FONT_HERSHEY_SIMPLEX,
		0.5, (127, 255, 0), 2)
        

    def showConvexHull(self): #not used - to send the entire contour of the figures picked up by camera
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

    def center(self): #finding the center using Moments
        M = cv2.moments(self.form)
        if M["m00"] == 0.0:
            M["m00"] = M["m00"] +1
        return (int(M["m10"] / M["m00"]), int(M["m01"] / M["m00"]))

    def getRadius(self):
        return self.radius

    def update(self): #updating the current position and orientation of the dancer and send points to Unity
        self.perimeter = cv2.arcLength(self.form, True)
        self.findLeftRightMost()
        self.history.appendleft(self.center()) #append initial center
        self.calculateVelocity()
        self.calculateDirection()
        self.calculateRadius()
        
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

    def draw(self): #to outline the person
        #self.showConvexHull()
        self.showLeftRightMost()
        drawContour(self.id,(0,255,0),self.form, self.diry)

    def calculateRadius(self): #to find the size of the person - for future use when have to find shoulders and hips etc
        '''Uses the bounding box to calculate pixel radius'''
        self.rect = cv2.boundingRect(self.form)
        x,y,w,h = self.rect
        self.radius = min(w/2, h/2)

    def calculateVelocity(self): #to find the speed of the dancer's movements - needed for effects in Unity
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

    def sendUDP(self): #function to send data to Unity *critical*
        '''message with id at begining'''
        x,y = self.center() #need to made into separate variables first
        lx,ly = self.leftmost
        rx,ry = self.rightmost
        hx,hy = self.extTop
        legLX, legLY = self.extBot
        legRX,legRY = self.otherLeg
        if (legRX < legLX) :
            temp = legRX
            legRX = legLX
            legLX = temp
            temp = legRY
            legRY = legLY
            legLY = temp

        '''
        print("Before conversion:")
        print("x,y: ", x, y)
        print("lx,ly: ", lx, ly)
        print("rx,ry: ", rx, ry)
        print("hx,hy: ", hx, hy)
        print("legLX,legLY: ", legLX, legLY)
        print("legRX,legRY: ", legRX, legRY)
        '''

        #Convert numpy int32 data to int data
        lx = np.int32(lx).item() 
        ly = np.int32(ly).item()
        rx = np.int32(rx).item()
        ry = np.int32(ry).item()
        hx = np.int32(hx).item()
        hy = np.int32(hy).item()
        legLX = np.int32(legLX).item()
        legLY = np.int32(legLY).item()
        legRX = np.int32(legRX).item()
        legRY = np.int32(legRY).item()

        '''
        print("After conversion:")
        print("x,y: ", x, y)
        print("lx,ly: ", lx, ly)
        print("rx,ry: ", rx, ry)
        print("hx,hy: ", hx, hy)
        print("legLX,legLY: ", legLX, legLY)
        print("legRX,legRY: ", legRX, legRY)

        print("x,y type: ", type(x), type(y))
        print("lx,ly type: ", type(lx), type(ly))
        print("rx,ry type: ", type(rx), type(ry))
        print("hx,hy type: ", type(hx), type(hy))
        print("legLX,legLY type: ", type(legLX), type(legLY))
        print("legRX,legRY type: ", type(legRX), type(legRY))
        '''
        
        
        # On unity side, may have to consider the wrist points as well
        # Feet points are sent in place of the hand points

        message = [ self.id, x,y,0, x,y,1, hx,hy,2, hx,hy,3, legLX,legLY,4, legLX,legLY,5, legLX,legLY,6, legLX,legLY,7, 
                    legRX,legRY,8, legRX,legRY,9, legRX,legRY,10, legRX,legRY,11,
                    legLX,legLY,12, legLX,legLY,13, legLX,legLY,14, legLX,legLY,15,
                    legRX,legRY,16, legRX,legRY,17, legRX,legRY,18, legRX,legRY,19,
                    x,y,20, legLX, legLY,21, legLX,legLY,22, legRX,legRY,23, legRX,legRY,24 
         ]    

        # 7 -> HandLeft in SimpleFrame, 11 -> HandRight in SimpleFrame
        # self.id, legLX, legLY, 7, legRX, legRY, 11          

        #print(type(message))
        print(message)
        mess = msgpack.packb(message)
        #                                    + str(lx) + "," + str(ly) + ","\
        #                                    + str(rx) + "," + str(ry) + ","\
        #                                    + str(hx) + "," + str(hy) + ","\
        #each variable needs to be sent as a string

        #Download msgpack -> https://pypi.python.org/pypi/msgpack-python, message has to match the following:
        '''
                public enum JointType{
                    SpineBase, SpineMid, Neck, Head, ShoulderLeft, ElbowLeft, WristLeft, HandLeft, ShoulderRight, ElbowRight, WristRight, HandRight, HipLeft, KneeLeft, AnkleLeft, FootLeft, HipRight, KneeRight, AnkleRight, FootRight, SpineShoulder, HandTipLeft, ThumbLeft, HandTipRight, ThumbRight
                }


                public class SimpleFrame{
                    public Dictionary<ulong, SimpleBody> Data = new Dictionary<ulong, SimpleBody>();
                }   

                public class SimpleJoint{
                    public UnityEngine.Vector2 Point = new Vector2();
                    public JointType Type;
                }

                public class SimpleBody{
                    public List<SimpleJoint> Joints = new List<SimpleJoint>();
                }
        '''

        #to add more point copy prev line syntax and add the points at str(pointx) and str(pointy)
        #mess = "DATA," + str(self.id) + "," + str(x) + "," + str(y) + ","\
        #                                    + str(legLX) + "," + str(legLY) + ","\
        #                                    + str(legRX) + "," + str(legRY) + ","\
        #                                    + str(self.radius) + "," + str(self.velocity)
        sock.sendto(mess , (UDP_IP, UDP_PORT))
        #print "sent " ,mess," to ",UDP_PORT, "For", self.id

    def calculateDirection(self): #for future use - not used currently
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
    #frame2 = 255-frame
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

def track(): #to keep the same body tracked - if lost, start fresh, dump old history
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

def colorFilter(src,lower,upper):
     # Threshold the HSV image to get only blue colors
    #Blur Kernel
    kernel = np.ones((3,3),np.uint8)
    imgMask = cv2.inRange(src, lower, upper)

    #Mask Inversion Handler
    if invert==1:
        imgMask = 255 - imgMask;
    else:
        imgMask = imgMask
    
    #morphological opening (removes small objects from the foreground)
    #Erode and Expand the edges of the mask to eliminate small artifacts
    imgMask = cv2.erode(imgMask, kernel, iterations=2)
    imgMask = cv2.dilate(imgMask, kernel, iterations=2)

    return imgMask



#Consts
UDP_IP = "127.0.0.1" #send to unity in the same machine
UDP_PORT = 9050 #unity listening at 9050 <- Port in Unity has to match
KEY_I = 105 #keyboard key defined for run time reset (initialisation) for GUI
KEY_C = 99 #run-time clear for GUI
KEY_ESC = 27 #esc defined for GUI
WAIT = 1
#Variables
is_initialized = False
cap = cv2.VideoCapture(0) #create an object of VideoCapture with device id as param
fname = "02_100.MP4"
flength = 100 # Length of video file in frames
loop = False #True if the video needs to be stored into fname = 02_100.MP4
colorTracking = True #If colorTracking is true, then will use for color tracking, otherwise will use IR tradking
if loop: #to record video
    cap = cv2.VideoCapture(fname)
c = 0  
formcount = 1 #Number of Forms to Expect (can be set through gui)
bodies = list() #Bodies Output
run = True
#GUI Defaults for IR tracking
thresh_l = 45
thresh_u = 255
brightness = 36
#GUI Defaults for Color Tracking
h_l, h_u = 0, 255#0,55
s_l, s_u = 0, 255
v_l, v_u = 0, 255#20,255
invert = 0

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

#sets the values got from the trackerBar into variables
def set_scale_h_u(val): 
    global h_u
    h_u = val
def set_scale_h_l(val):
    global h_l
    h_l = val
def set_scale_s_u(val):
    global s_u
    s_u = val
def set_scale_s_l(val):
    global s_l
    s_l = val
def set_scale_v_u(val):
    global v_u
    v_u = val
def set_scale_v_l(val):
    global v_l
    v_l = val

def set_invert_mask(val):
    global invert 
    invert = val

cv2.namedWindow('control panel', 0)
cv2.createTrackbar('is_initilized', 'control panel', 0, 1 , set_gui_initialized) #trackbar for initialise
cv2.createTrackbar('formcount', 'control panel', 1, 9 , set_gui_formcount) #trackbar for set number of dancers/forms
if colorTracking:
    print "color tracking mode" #caliberation for hue limits, sat limits, invertion required or not 
    cv2.createTrackbar('Hue_lower', 'control panel', 0, 255, set_scale_h_l) 
    cv2.createTrackbar('Hue_upper', 'control panel', 255, 255, set_scale_h_u)
    cv2.createTrackbar('Sat_lower', 'control panel', 0, 255, set_scale_s_l)
    cv2.createTrackbar('Sat_upper', 'control panel', 255, 255, set_scale_s_u)
    cv2.createTrackbar('Val_lower', 'control panel', 0, 255, set_scale_v_l)
    cv2.createTrackbar('Val_upper', 'control panel', 255, 255, set_scale_v_u)
    cv2.createTrackbar('Invert Mask', 'control panel', 0, 1, set_invert_mask)
else:
    print "ir tracking mode" #caliberate the threshold to convert IR image captured to black and white image
    cv2.createTrackbar('thresh_lower', 'control panel', 0, 255, set_scale_thresh_l)
    cv2.createTrackbar('thresh_upper', 'control panel', 255, 255, set_scale_thresh_u)
cv2.createTrackbar('src brightness', 'control panel', 36, 255 , set_scale_brightness) 
cv2.createTrackbar('Exit', 'control panel', 0, 1 , set_gui_exit)
### END GUI

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
    if colorTracking:
        lower = np.array([h_l,s_l,v_l]) 
        upper = np.array([h_u,s_u,v_u])

        thresh = colorFilter(frame,lower,upper)
    else:
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
