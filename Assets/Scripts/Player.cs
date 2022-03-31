using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace HelloWorld
{
    public class Player : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public NetworkVariable<Color> ColorPlayer = new NetworkVariable<Color>();
        public static List<Color> listaColores = new List<Color>();
        Renderer rend;
             
        void Start() {       
            rend = GetComponent<MeshRenderer>();        
            Position.OnValueChanged += OnPositionChange;
            ColorPlayer.Value = listaColores[2];
        }

        public void llenarListaColores(){
            listaColores.Add(Color.blue); 
            listaColores.Add(Color.red);
            listaColores.Add(Color.white);       
        }

        public void OnPositionChange(Vector3 previousValue, Vector3 newValue){
            transform.position = Position.Value;
        }

        public override void OnNetworkSpawn()
        {

            if(IsServer && IsOwner){
                llenarListaColores();
            } 
            if (IsOwner)
            {
                TeamChange1();
                TeamChange2();
                NoTeamChange();
            }
        }

        public void TeamChange1(){        
            SubmitColorTeam1RequestServerRpc();        
        }

        public void TeamChange2(){  
            SubmitColorTeam2RequestServerRpc();               
        }

        public void NoTeamChange(){
            SubmitColorNoTeamRequestServerRpc();
        }
        
        [ServerRpc]
        void SubmitColorTeam1RequestServerRpc(ServerRpcParams rpcParams = default)
        {             
            Position.Value = new Vector3(Random.Range(-0.5f, -9f), 1f, Random.Range(-3f, 4f));
            ColorPlayer.Value = listaColores[0];         
        }

        [ServerRpc]
        void SubmitColorTeam2RequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = new Vector3(Random.Range(10f, 18f), 1f, Random.Range(-3f, 4f));
            ColorPlayer.Value = listaColores[1];        
        }

        [ServerRpc]
        void SubmitColorNoTeamRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = new Vector3(Random.Range(0.6f, 9f), 1f, Random.Range(-3f, 4f));
            ColorPlayer.Value = listaColores[2];       
        }

        void Update()
        {
            rend.material.color = ColorPlayer.Value;
        }
    }
}