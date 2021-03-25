# Travelling Salesmen Problem
--PORTUGUESE--
COMO USAR:
1) Selecione o GameObject SimulationManager na hierarquia.
2) Insira, no inspector, o número desejado nos campos "Qnt Cidades" (Número de cidades geradas), "Populacao" (Número de indivíduos), e "Limite Geracoes" (Limite de geração ou condição de fim).
3) Certifique que as quantidades de todas essas variáveis são no mínimo maior que 2.
4) Clique em Play no Editor.

COMO FUNCIONA A LÓGICA:
1) O algorítimo cria, em posições aleatórias, o número de cidades especificado no inspector.
2) É associado uma ID a cada cidade, e cada cidade é colocada dentro de uma única lista. A cidade 0 é definida como origem por convenção.
3) Conforme a quantidade de população inserida no inspector, é criado uma lista de indivíduos chamados "Rota", na qual cada rota guarda uma sequência de cidades aleatórias, onde a primeira é sempre a cidade 0. Esta é a população inicial.
4) Antes de começar a simulação, o algorítimo certifica e garante que não haja indivíduos de mesmo dna na primeira população.
5) Quando todos os indivíduos forem diferentes, a simulação começa e calcula a distância de cada indivíduo Rota, e seleciona as duas melhores rotas, ou seja, com menor distância.
6) É feito o crossover entre a primeira e segunda melhor rota, utilizando o modelo Ordered Crossover (OX1), pois não é permitido repetir cidades.
7) Logo após, existe 20% de chance de alguma rota aleatória seja selecionada para sofrer mutação, onde são trocados de posições dois dnas aleatórios.
8) Por fim, a pior rota é eliminada (a que tiver maior distância), mantendo o número de população.
9) A fase 5 até 8 são repetidas em loop até que a geração atinja o limite de gerações especificado no inspector.

--ENGLISH--
HOW TO USE:
1) Select SimulationManager in hierachy.
2) Insert, on inspector, the desired number in the fields "Qnt Cidades" (Number of generated cities), "Populacao" (Number of individuals), e "Limite Geracoes" (Generation limit or simulation end condition).
3) Certify that the quantities of all these variables are minimum greater than 2.
4) Click Play in Editor.

HOW LOGIC WORKS:
1) The algorithm creates, in random positions, the number of cities specified in the inspector.
2) An ID is associated with each city, and each city is placed within a single list. City 0 is defined as origin by convention.
3) Depending on the amount of population entered in the inspector, a list of individuals called "Route" is created, in which each route stores a sequence of random cities, where the first one is always the city 0. This is the initial population.
4) Before starting the simulation, the algorithm certifies and guarantees that there are no individuals of the same DNA in the first population.
5) When all individuals are different, the simulation starts and calculates the distance of each individual Route, and selects the two best routes, that is, with the shortest distance.
6) A crossover is made between the first and second best routes, using the Ordered Crossover model (OX1), as it is not allowed to repeat cities.
7) Right after, there is a 20% chance that some random route will be selected to undergo mutation, where two random dna positions are switched.
8) Finally, the worst route is eliminated (the one with the longest distance), maintaining the number of population.
9) Phases 5 through 8 repeats until the generation number reaches the generation limit specified in the inspector.
