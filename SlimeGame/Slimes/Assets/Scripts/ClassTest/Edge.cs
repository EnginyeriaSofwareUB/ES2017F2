using System.Collections.Generic;

class Edge<N>{

    private N node;

    private float weight;

    private bool direction;

    public Edge(N node,bool d){
        this.node = node;
        weight = -1.0f;
        direction = d;
    }

     public Edge(N node,bool d,float w){
        this.node = node;
        weight = w;
        direction = d;
     }

    //TODO: Change this to a real swapper
    public void ChangeNodes(N n1,N n2){
        //node1 = n1;
        //node2 = n2;
    }

    public N getNext(){
        return node;
    }

}