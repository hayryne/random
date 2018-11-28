#!/usr/bin/env python3
# coding: utf-8

import sys
from ast import literal_eval as luopiste

LEVEYS, KORKEUS = (5,5)
ALKUPAIKKA = (0,0)
SIIRROT = [ (1,2),(1,-2),(-1,2),(-1,-2),
            (2,1),(2,-1),(-2,1),(-2,-1) ]

def luolauta(koko):
    return [[0 for ruutu in range(LEVEYS)] for rivi in range(KORKEUS)]
    
def tulostalauta(lauta):
    for rivi in lauta:
        for ruutu in rivi:
            print(format(ruutu, '02d') + ' ', end='')
        print()
        
def ratsasta(lauta, paikka, askel):
    x, y = paikka
    lauta[y][x] = askel
    if askel == LEVEYS * KORKEUS:
        tulostalauta(lauta)
        exit()
    for deltax, deltay in SIIRROT:
        uusipaikka = (x+deltax, y+deltay)
        if saakosiirtya(lauta, uusipaikka):
            ratsasta(lauta, uusipaikka, askel+1)
    lauta[y][x] = 0
    if askel == 1:
        print('Ratkaisua ei ole olemassa.')
        exit()
    
def saakosiirtya(lauta, paikka):
    x, y = paikka
    return 0 <= x < LEVEYS and 0 <= y < KORKEUS and lauta[y][x] == 0

def main():
    global LEVEYS, KORKEUS, ALKUPAIKKA
    
    try:
        if len(sys.argv) > 1:
            koko = luopiste(sys.argv[1])
            if min(koko) > 0:
                LEVEYS, KORKEUS = koko
        if len(sys.argv) > 2:
            x, y = luopiste(sys.argv[2])
            if 0 <= x < LEVEYS and 0 <= y < KORKEUS:
                ALKUPAIKKA = (x,y)
    except:
        print('Virheellinen syöte.')
        exit()
        
    lauta = luolauta((LEVEYS,KORKEUS))
    ratsasta(lauta, ALKUPAIKKA, 1)

main()