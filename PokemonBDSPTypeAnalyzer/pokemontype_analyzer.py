from enum import auto
from enum import StrEnum
import json 

class Type(StrEnum):
    GRASS = auto()
    WATER = auto()
    FIRE = auto()
    ELECTRIC = auto()
    ICE = auto()
    FIGHTING = auto()
    GROUND = auto()
    POISON = auto()
    FLYING = auto()
    PSYCHIC = auto()
    BUG = auto()
    ROCK = auto()
    GHOST = auto()
    DRAGON = auto()
    DARK = auto()
    STEEL = auto() 
    FAIRY = auto()
    NORMAL = auto()

    def __str__(self):
        return self.name[0].upper() + self.name[1:].lower()

ADVANTAGES = {
    'Grass': ['Ground', 'Rock', 'Water'],
    'Water': ['Fire', 'Ground', 'Rock'],
    'Fire': ['Bug', 'Grass', 'Ice', 'Steel'],
    'Electric': ['Flying', 'Water'],
    'Ice': ['Grass', 'Ground', 'Flying', 'Dragon'],
    'Fighting': ['Dark', 'Ice', 'Normal', 'Steel', 'Rock'],
    'Ground': ['Electric', 'Fire', 'Poison', 'Rock', 'Steel'],
    'Poison': ['Grass', 'Fairy'],
    'Flying': ['Bug', 'Fighting', 'Grass'],
    'Psychic': ['Fighting', 'Poison'],
    'Bug': ['Dark', 'Grass', 'Psychic'],
    'Rock': ['Bug', 'Flying', 'Fire', 'Ice'],
    'Ghost': ['Psychic', 'Ghost'],
    'Dragon': ['Dragon'],
    'Dark': ['Psychic', 'Ghost'],
    'Steel': ['Fairy', 'Ice', 'Rock'],
    'Fairy': ['Dark', 'Dragon', 'Fighting'],
    'Normal': []
}
WEAKNESSES = {
    'Fighting': ['Flying', 'Psychic', 'Fairy'], 
    'Rock': ['Grass', 'Water', 'Fighting', 'Ground', 'Steel'], 
    'Fairy': ['Poison', 'Steel'], 
    'Ground': ['Grass', 'Water', 'Ice'], 
    'Poison': ['Ground', 'Psychic'], 
    'Steel': ['Fire', 'Fighting', 'Ground'], 
    'Normal': ['Fighting'], 
    'Psychic': ['Bug', 'Ghost', 'Dark'], 
    'Bug': ['Fire', 'Flying', 'Rock'], 
    'Fire': ['Water', 'Ground', 'Rock'], 
    'Ghost': ['Ghost', 'Dark'],
    'Dragon': ['Ice', 'Dragon', 'Fairy'], 
    'Electric': ['Ground'], 
    'Dark': ['Fighting', 'Bug', 'Fairy'], 
    'Grass': ['Fire', 'Ice', 'Poison', 'Flying', 'Bug'], 
    'Water': ['Grass', 'Electric'], 
    'Ice': ['Fire', 'Fighting', 'Rock', 'Steel'], 
    'Flying': ['Electric', 'Ice', 'Rock']
  }

MOVE_TYPES = {
    'Thunderbolt': 'Electric',
    'Ice Beam': 'Ice', 
    'Psychic': 'Psychic', 
    'Drain Punch': 'Fighting',
    'Energy Ball': 'Grass',
    'Dazzling Gleam': 'Fairy', 
    'Flamethrower': 'Fire',
    'U-Turn': 'Bug',
    'Shadow Ball': 'Ghost'
}

types = set(map(lambda type: str(type), list(Type)))

def moveset_advantages(movetypes):
    type_advantages = {}
    for movetype in movetypes:
        for advantage in ADVANTAGES[movetype]:
            if advantage not in type_advantages.keys():
                type_advantages[advantage] = []
            type_advantages[advantage].append(movetype)
    return type_advantages

class Moveset:
    def __init__(self, moves):
        self.moves = moves

    def __str__(self):
        advantages = moveset_advantages(list(map(lambda move: MOVE_TYPES[move], self.moves)))
        missing = types - advantages.keys()

        s = '/'.join(self.moves) + ':\n'
        s += f'\t[{str(len(advantages))} type advantages]\n' 
        for a in advantages.keys():
            s += f'\t\t{a} [from ' + ','.join(advantages[a]) + "]\n"    
        s+= ','.join(missing)
        return s

azelf1 = Moveset(['Thunderbolt', 'Drain Punch', 'Psychic', 'Flamethrower'])
azelf2 = Moveset(['Energy Ball', 'Drain Punch', 'Psychic', 'Flamethrower'])
azelf3 = Moveset(['Energy Ball', 'Drain Punch', 'Psychic', 'U-Turn'])
mesprit1 = Moveset(['Ice Beam', 'Drain Punch', 'Psychic', 'Shadow Ball'])


print(azelf1)
print(azelf2)
print(azelf3)
print(mesprit1)