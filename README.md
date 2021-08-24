# Evolution-of-particle-systems

We try to simulate large crowds of people in urban environments.
## Table of Contents

* [Project Description](#project-description)
* [Meeting notes 11.05.2021](#meeting-notes-11.05.2021-(German))
* [Meeting notes 23.08.2021](#meeting-notes-23.08.2021-(German))
* [Technologies](#technologies)
* [TODO](#TODO)
* [Project Status](#project-status)
* [Inspiration](#inspiration)
* [Resources](#resources)
* [Contributors](#contributors)

## Project Description
In this project we want to simulate a big urban enviroment. 


## Meeting notes 11.05.2021 (German)
* Chemotaxis für Ameisen (Alle laufen in Richtung des höchsten Gradienten -> Weg des geringsten Widerstandes)
* Simulation: Simulationstechniken
* Multiagentensysteme
* Anwendungen: Straßennetz passt am ehesten
* Kleine Stadt bauen per Zufallsgenerator und dann entwickeln sich Wege (Patrick Henning),
unterschiedliche Größen mit Eingängen und dann einfach mal simulieren lassen. Verschiedene
Populationen, Anzahl, Interessen etc.
* Brainstorming machen und dann entscheiden wie wir weiter machen
* Erst danach konkrete Pläne
* Open Street Maps für Lagepläne
* Wie dicht muss ich Knoten auf die Fläche packe, wenn man alle Bewegungen erlaubt
* Das nächste Mal mit Demo


## Meeting notes 23.08.2021 (German)
* Straßensystem entstehen da wo die meisten laufen
* Agentensystem kann Planungshilfe sein -> lassen Leute mit Busen ankommen und zu entsprechenden Zeiten und dann schauen was passiert
* Einzelne Gebäude haben Zeiten an denen Leute kommen -> versuchen an realistische Szenarien rankommen
* Verschiedene Eingänge zu Mensa und Gebäude und wie entzerrt man Trauben von Leuten vor den Eingängen
* Vorschläge für Verbesserung von Wegen und wie könnte man ein bestehenden System verbessern -> auflösen von Blockaden
* Agentensysteme werden eingesetzt um bestimmtes Laufverhalten vorher schon zu prognostizieren und dann Wege entsprechend anlegen
* Gebäude als ein Objekt mit mehreren Eingägnen und dann kann der Agent auch hin und her wechseln
* Sehr konkrete zum steuern von Verkehrssystem herangezogen werden kann
* bestimmte Stoßzeiten und Haltestellen
* bestimmten Tag durchrechnen am Neuenheimer Feld
* Wege besser finden -> Einfluss von bestimmten zeitlichen Rythmen in den Gebäuen sieht
* Wegen werden "ausgetretener" 
* Individuelle Agentenzeiten 
* Überlegen wie diese dann ihre Spuren hinterlassen
* Gibt eher eine Art Rückstau der sich dann auch verfärbt und anhand davon Rückschlüsse auf "gefährliche" Gebilde
* Drei vier verschiedene Sachen die wir einbauen wollen, das man neue Busliene durch Feld plant und wie würde es alles verändern -> mögliche Entlastung 
* Erst Grundsätzlich 3d Modell und dann wie wir vorgegangen sind
* Können beim Vortragen hinweisen, das und das haben wir entschieden und was hatte das für Auswirkungen 
* Wie Unity intern vorgeht, herausfinden und dann in der Präsentation besprehcen
* Informatische Einordung der Simulation, Vergleich mit Wegfindungsalgorithmen
* Auch theoretischen Teil der Simulation erklären und wie können wir die Performance verbessern
* Konzentrierne auf ein Szenario
* Workflow vermitteln, warum wir das Szenario haben und keine anderne Szenarien einbauen
* Wo wollten man schon eine Straßenbahn bauen -> Zeitung informieren
* Workflow reicht und jeder kann dann selber erweitern 
* Erst laufen lassen und simulierne und warum kommen wir an Leitungsgrenzen?
* Dann eingreifen und Situation verändern und dann wie die Simulation sich verändert
* Zusammenfassen in einem Standbild 
* Agents hinterlassen Spuren und diese bilden dann das Ergebnisbild 
* Vorschlag wo man Straßen baut und wie man sie bauen müsste
* Komplexitätsgedanken und wie das System darauf reagiert
* können testen, ob 100 Agenten reichen, da einer für das Massenverhatlen für 100 weitere steht
* schön, wenn wir ein Resultat haben -> nicht nur Neuenheimer Feld und Agenten laufen, sondern eher Wegenetz als Resultat und dann schauen und verlgeichen zwischen veränderten und unveränderten
* Ziele setzen und dann schauen wie es weitergeht
* Einfärben und Wegenetz und dann verändert sich das Wegenetz
* Emergency Fälle möglich zur Simulation
* können das auch versuchen -> wie reagieren Agenten auf Feuer etc. 
* Ziel selber stecken und wenn alternative Möglichkeiten Ziel zu erreichen und dann im Unterschied wird deutlich was
* Sollten viel Arbeit reinstecken, da 8 LP
* Validierungssystem mit berücksichtigen
* Quantitativ oder qualitativ
* Tabelle erstellen und Vergleiche ziehen, zb mit unterschiedlichen Agent Anzahl etc. 
* Austesten was bei kleinen Systemen passiert
* Folien schon vorbereiten und dann schauen ob wir alles haben
* Nächstes Mal PDF und 


## Technologies 
* Unity Version 2020.3.12f1

## TODO
* Look in the [Projects tab](https://github.com/JanMStraub/Evolution-of-particle-systems/projects/3).

## Project Status

### 23.06.2021
Finished first commute routine test with 500 agents.

![First commute test with 500 agents](git_res/commute_test.gif)
- - - - -

### 10.07.2021
Finished prototype simulation and tested it with 700 agents.

![First test with 700 agents](git_res/prototype_test.gif)
- - - - -


### 14.07.2021
Implemented NavMesh spawn randomizer to improve performance.
Also tried to deaktivate collision detection, but that caused other problems.

![Test with 2000 agents, without collision detection](git_res/prototype_test_2000_no-collision.gif)

![Test with 2000 agents, with collision detection](git_res/prototype_test_2000.gif)


## Inspiration

* [Sebastian Lague - Coding Adventure Ant and Slime Simulations](https://www.youtube.com/watch?v=X-iSQQgOd1A&t)

## Resources



## Contributors

* Paavo Streibich, paavo.streibich@uni-heidelberg.de

* Jan Straub, jan.straub@stud.uni-heidelberg.de