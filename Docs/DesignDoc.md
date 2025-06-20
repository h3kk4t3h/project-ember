## Capa

**Título:** ARPG isometrico em Unity: Do Conceito à Implementação

**Autores:** Bianca Ribeiro, Lucas Quiroz, Ricardo Roda, Rui Moreira

**Curso:** 5425 - Projeto de Tecnologias e Sistemas de Informação

**Data:** 19 de junho de 2025

---

## Resumo

Este relatório descreve o desenvolvimento de um jogo Action RPG (ARPG) usando a Unity Engine. O objetivo é criar um protótipo funcional inspirado em títulos como Diablo, implementando sistemas de progressão de nível, inventário, árvore de habilidades e controlo de movimento e combate. A metodologia adotada baseia-se em práticas de desenvolvimento ágil, utilizando diagramas UML para a fase de design e testes unitários e de integração para garantir a qualidade do código.

---

## Índice

1. Introdução
2. Estado da Arte
3. Objetivos
4. Metodologia
5. Design do Jogo
6. Implementação
7. Modelo de Dados
8. Testes e Validação
9. Cronograma Detalhado
10. Critérios de Sucesso e Métricas
11. Conclusões e Trabalhos Futuros
12. Referências
13. Anexos

---

## 1. Introdução

Os jogos ARPG têm conquistado popularidade pela combinação de ação em tempo real e progressão de personagem. Este projeto pretende explorar o pipeline completo de desenvolvimento, desde a conceção do design até à implementação técnica em C# e Unity.

---

## 2. Estado da Arte

- **Diablo (Blizzard North, 1996):** pioneiro em ARPG com sistema de loot e progressão de personagem.
- **Torchlight (Runic Games, 2009):** ARPG 2D/2.5D com foco em exploração de masmorras e estética vibrante.
- **Legends of Idleon:** exemplo de ARPG 2D em browser, enfatiza sistemas de progressão automáticos.

As engines mais comuns para 2D incluem Unity e Godot. C# na Unity oferece ampla comunidade e recursos.

---

## 3. Objetivos

### Objetivo Geral

Desenvolver um protótipo de ARPG isometrico com funcionalidades-chave de progressão, inventário, árvore de habilidades e combate.

### Objetivos Específicos

- Implementar sistema de experiência (XP) e níveis.
- Criar interface de inventário com suportes a itens equipáveis e consumíveis.
- Desenvolver árvore de habilidades com atribuição de pontos de talento.
- Implementar movimento em 8 direções e sistema de colisões para combate.

---

## 4. Metodologia

### Abordagem de Desenvolvimento

Utilização de Scrum com sprints de duas semanas, reuniões diárias de stand-up e revisões ao final de cada sprint.

### Ferramentas e Ambiente

- **Unity 6**
- **Linguagem:** C# (.NET Standard)
- **IDE:** Rider e Visual Studio
- **Versionamento:** Git e GitHub
- **Comunicação:** Discord, Notion e GitHub Projects para gestão de tarefas

---

## 5. Design do Jogo

### 5.1 Diagramas UML

- **Caso de Uso:** Player navega pelo mapa, ganha XP ao derrotar inimigos e usa inventário.
- **Classes Principais:** PlayerController, InventoryManager, SkillTree, EnemyAI, Item, UIManager.

### 5.2 Wireframes e Mockups

- Mockups da HUD: barras de vida, XP, slots de inventário.
- Layout da árvore de habilidades.

---

## 6. Implementação

### 6.1 Arquitetura de Software

Adotado padrão MVC para separação de lógica de jogo (Model), apresentação (View) e controlo (Controller). Uso de ScriptableObjects para dados de itens e habilidades.

### 6.2 Principais Scripts

- **PlayerController.cs:** Lê input do teclado, aplica movimento e animações.
- **InventoryManager.cs:** Gerencia adição, remoção e uso de itens, atualiza UI.
- **SkillTreeSystem.cs:** Processa desbloqueio de skills, alocação de pontos.
- **ExperienceSystem.cs:** Calcula XP, gatilha eventos de nível.

---

## 7. Modelo de Dados

### 7.1 Entidades e Atributos

- *Player (PlayerID, Nivel, XP, Vida, Mana)
- *Item (ItemID, Nome, Tipo, Efeito)
- *Skill (SkillID, Nome, Descricao, CustoPontos)
- *Inventário (PlayerID, ItemID, Quantidade)

### 7.2 Diagrama ER

```
Player 1:N Inventário N:1 Item
Player 1:N ExperienceLog
Player 1:N SkillAllocation N:1 Skill
```

---

## 8. Testes e Validação

- **Testes Unitários:** NUnit para métodos de cálculo de XP e gestão de inventário.
- **Testes de Integração:** Cenas de debug com scripts de teste.
- **Playtests:** Membros do curso

---

## 9. Cronograma Detalhado

| Sprint | Duração | Objetivos |
| --- | --- | --- |
| 1 | 2 sem | Documentação e design UML |
| 2 | 2 sem | PlayerController e sistema de movimento |
| 3 | 2 sem | InventoryManager e UI |
| 4 | 2 sem | ExperienceSystem e UI de níveis |
| 5 | 2 sem | SkillTreeSystem |
| 6 | 2 sem | Testes, debugging e polimento geral |

---

## 10. Critérios de Sucesso e Métricas

- Protótipo com todas as features principais implementadas.
- Desempenho mínimo: 60 FPS em hardware médio.
- Taxa de bugs críticos inferior a 1 por hora de uso.
- Satisfação média dos jogadores ≥ 4/5.

---

## 11. Conclusões e Trabalhos Futuros

O protótipo demonstrou viabilidade técnica e aceitação positiva em testes iniciais. Para versões futuras, recomenda-se:

- Adição de sistema de quests.
- Implementação de efeitos sonoros e música original.
- Multiplayer local ou online.
- Geração procedural de mapas.

---

## 12. Referências

- Blizzard Entertainment. *Diablo*. 1996.
- Unity Technologies. *Unity Manual*. 2025.
- R. Smith. *Game Programming Patterns*. 2014.

---

## 13. Anexos

- Excertos de código relevantes.
- Capturas de ecrã do protótipo.
- Questionário de feedback dos utilizadores.
