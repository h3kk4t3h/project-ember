# Project Ember

> Este documento descreve o ciclo de jogo do Project Ember, um jogo onde os jogadores podem escolher diferentes personagens e habilidades para derrotar inimigos em vários mapas.

## Menu

- O jogador pode escolher entre iniciar um novo jogo ou carregar um jogo existente.
- O jogador pode ter vários personagens, cada um com a sua própria progressão e classe.

## Gameplay Loop

### Criação de Personagem

- O jogador escolhe a classe do seu personagem, que define as suas habilidades e estilo de jogo.

### Town Hub

- O jogador inicia o jogo e é apresentado ao town hub, onde pode ver as suas habilidades, stats e a skill tree.
- No town hub existem vendors e um portal onde pode selecionar o mapa e a sua dificuldade, que está relacionada com as recompensas e com a progressão.
- Nos vendors o jogador pode comprar e vender itens com ouro obtido nos mapas.
- O jogador pode também trocar de personagem, com progressão separada.

### Mapa

- O jogador movimenta-se pelo mapa, eliminando inimigos com as suas habilidades. Os inimigos são gerados no mapa, e o objetivo é eliminá-los o mais rapidamente possível para obter as melhores recompensas.
- As recompensas são modificadores para as habilidades e experiência.
- A experiência contribui para o nível do jogador e o nível influencia os seus stats (vida, dano, mana) e dá-lhe pontos para atribuir na skill tree.
- Os modificadores influenciam as propriedades das habilidades (dano, área de dano, cooldown, velocidade, consumo de mana, número de projéteis).
- No final de cada mapa, o jogador regressa ao town hub e pode escolher entre 3 recompensas.

## Progressão

- Nivel da personagem
- Skill tree
- Sequência de mapas
- Dificuldade dos mapas
- Items

## Stats & Attributes

-- Base Stats:
- Health
- Mana
- Mana regen/s
- Level
- Current XP (Talvez criamos uma tabela in game para sabermos o xp por nivel)
- Gold

-- Attributes:
- Strength -> Crit chance & Ailment Chance
- Intelligence -> Mana & Cooldown reduction
- Dexterity -> Attack/Cast Speed & Move Speed
- Vitality -> HP & Armor Threshold

-- Damage Types: (Ailment)
- Fire (Burn)
- Water (Chill)
- Air (Shock)
- Earth (Stun)
- Arcane (Void)

-- Damage stats:
- Critical Chance (percentage)
- Critical Multiplier (percentage)
- Damage Multiplier (per damage type) (percentage)
- Damage Flat (per damage type)
- Ailment Chance (percentage)
- Ailment Multiplier (percentage)
- Attack/Cast Speed (percentage)
- Cooldown reduction (percentage)

- Armor stats:
Armor
Armor Threshold
Armor Regen (quantidade que se recupera depois do tempo de espera)
Move Speed 

