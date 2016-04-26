#Provides a simple interface for streaming Video Data to unity.
#
#Simply run like: test.py data/<filename> 
#                 and it will stream the data to unity 
#                 in a loop via localhost port 5005 so 
#                 you wont need to open a video stream
#
#                 Tests will autoload formcount and speed data. Speed data is loaded into this script to prevent bottlenecking of messages and to make the input look more naturalistic. formcount data is loaded unity side and used as a que to re-initialize data streami

import sys
import time
import socket

def sendUDP(mess):
    '''message with id at begining'''
    sock.sendto(mess , (UDP_IP, UDP_PORT))

UDP_IP = "127.0.0.1"
UDP_PORT = 5005


sock = socket.socket(socket.AF_INET, # Internet
                 socket.SOCK_DGRAM) # UDP

fo = open("tests/1","r")
lines = fo.readlines()

count = 0
mc = 3
debug = False

speed = 1

if not fo:
    print "Incorrect Filename"
    exit()

while (True):
    for l in lines:
        if l == "END" and debug:
            count += 1
        if l[:5] == "SPEED":
            speed = float(l[5:])
        if count == mc and debug:
            break
        time.sleep(speed)
        sendUDP(l)
        if count == mc and debug:
            break
#Close Socket`
sock.close()
