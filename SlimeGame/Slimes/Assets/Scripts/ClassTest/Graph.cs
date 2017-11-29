using System.Collections.Generic;

class Graph<T>{
    private Node<T> _DefaultStart;
    private Node<T> First;

    private List<Node<T>> NodeList;

    public Node<T> first{
        get{
            return First;
        }
        set{
            if(_DefaultStart==null){
                _DefaultStart = value;
            }
            first = value;
        }
    }

    public Graph(){
        NodeList = new List<Node<T>>();
    }

    public Node<T> AddEmptyNode(){
        Node<T> n = new Node<T>();
        return n;
    }

    public Node<T> AddNode(T data){
        Node<T> n = new Node<T>(data);
        return n;
    }

    public void AddEdge(int nodeIdx1,int nodeIdx2){
        Node<T> n1 = NodeList[nodeIdx1];
        Node<T> n2 = NodeList[nodeIdx2];
        
        if(n1 != null && n2 != null){
            n1.AddEdge(n2);
            n2.AddEdge(n1);
        }
    }

    public void AddEdge(Node<T> node1,Node<T> node2){
        if(node1 != null && node2 != null){
            node1.AddEdge(node2);
            node2.AddEdge(node1);
        }
    }

    public void AddUniDirectonalEdge(int nodeIdx1,int nodeIdx2){
        Node<T> n1 = NodeList[nodeIdx1];
        Node<T> n2 = NodeList[nodeIdx2];
        
        if(n1 != null && n2 != null){
            n1.AddEdge(n2,true);
            n2.AddEdge(n1,false);
        }
    }

    public void AddUniDirectonalEdge(Node<T> node1,Node<T> node2){
        if(node1 != null && node2 != null){
            node1.AddEdge(node2,true);
            node2.AddEdge(node1,false);
        }
    }

    public void AddWeightedEdge(int nodeIdx1,int nodeIdx2,float weight){
        Node<T> n1 = NodeList[nodeIdx1];
        Node<T> n2 = NodeList[nodeIdx2];
        
        if(n1 != null && n2 != null){
            n1.AddEdge(n2,weight);
            n2.AddEdge(n1,weight);
        }
    }

    public void AddWeightedEdge(Node<T> node1,Node<T> node2,float weight){
        if(node1 != null && node2 != null){
            node1.AddEdge(node2,weight);
            node2.AddEdge(node1,weight);
        }
    }
    
    public void setFirstNode(Node<T> node){
        First = node;
    }


}