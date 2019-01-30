from collections import defaultdict

class Graph:
    def __init__(self, vertices):
        self.V = vertices
        self.graph = defaultdict(list)


    def add_edge(self, u, v):
        self.graph[u].append(v)


    def printAllPathsUtil(self, u, d, visited, path):
        # Mark the current node as visited and store in path
        visited[u]= True
        path.append(u)

        # If current vertex is same as destination, then print
        # current path[]
        if u == d:
            if len(path) == 5:
                print(path)
        else:
            # If current vertex is not destination
            #Recur for all the vertices adjacent to this vertex
            for i in self.graph[u]:
                if not visited[i]:
                    self.printAllPathsUtil(i, d, visited, path)

        # Remove current vertex from path[] and mark it as unvisited
        path.pop()
        visited[u]= False


    # Prints all paths from 's' to 'd'
    def printAllPaths(self,s, d):

        # Mark all the vertices as not visited
        visited =[False]*(self.V)

        # Create an array to store paths
        path = []

        # Call the recursive helper function to print all paths
        self.printAllPathsUtil(s, d,visited, path)

g = Graph(5)
g.add_edge(0, 1)
g.add_edge(0, 2)
g.add_edge(0, 3)
g.add_edge(1, 0)
g.add_edge(1, 2)
g.add_edge(1, 3)
g.add_edge(2, 0)
g.add_edge(2, 1)
g.add_edge(2, 3)
g.add_edge(3, 0)
g.add_edge(3, 1)
g.add_edge(3, 2)

g.add_edge(4, 0)
g.add_edge(1, 4)
g.add_edge(2, 4)
g.add_edge(3, 4)

s = 0
d = 4
g.printAllPaths(s, d)
