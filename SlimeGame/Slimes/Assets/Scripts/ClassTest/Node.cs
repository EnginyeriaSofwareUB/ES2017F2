using System.Collections.Generic;
using System;

class Node<T>{

    private T Data;
    private List<Edge<Node<T>>> Edges;

    public T data{
        get{
            return Data;
        }
        set{
            Data = value;
        }
    }

    public Node(){
        Edges = new List<Edge<Node<T>>>();
    }

    public Node(T data){
        Edges = new List<Edge<Node<T>>>();
        this.Data = data;
    }

    public Edge<Node<T>> AddEdge(Node<T> n){
        Edges.Add(new Edge<Node<T>>(n,true));
        return Edges[Edges.Count-1];
    }

    public Edge<Node<T>> AddEdge(Node<T> n,float w){
        Edges.Add(new Edge<Node<T>>(n,true,w));
        return Edges[Edges.Count-1];
    }

    public Edge<Node<T>> AddEdge(Node<T> n,bool d){
        Edges.Add(new Edge<Node<T>>(n,d));
        return Edges[Edges.Count-1];
    }

    public Edge<Node<T>> AddEdge(Node<T> n,bool d, float w){
        Edges.Add(new Edge<Node<T>>(n,d,w));
        return Edges[Edges.Count-1];
    }

    public List<Node<T>> getNeighbours(){
        List<Node<T>> l = new List<Node<T>>();
        foreach(Edge<Node<T>> e in Edges){
            l.Add(e.getNext());
        }
        return l;
    }
}