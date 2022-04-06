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

        public static List<int> teamMembers = new List<int>();

        public NetworkVariable<int> teamPlayer = new NetworkVariable<int>();


        private int maxPlayers = 2;
       
        Renderer rend;
             
        void Start() {       
            rend = GetComponent<MeshRenderer>();        
            Position.OnValueChanged += OnPositionChange;
            ColorPlayer.OnValueChanged += OnColorChange;    
                  
        }

        public void llenarListaColores(){
            listaColores.Add(Color.white);
            listaColores.Add(Color.blue); 
            listaColores.Add(Color.red);
                   
        }

        public void OnPositionChange(Vector3 previousValue, Vector3 newValue){
            transform.position = Position.Value;
        }

        public void OnColorChange(Color oldColor, Color newColor){
            rend.material.color = ColorPlayer.Value;
        }

        public override void OnNetworkSpawn()
        {

            if(IsServer && IsOwner){
                llenarListaColores();
            } 
            if (IsOwner)
            {
                Team(-1);
            }
        }

        public void Team(int equipo){        
            SubmitTeamRequestServerRpc(equipo);        
        }

        [ServerRpc]
        void SubmitTeamRequestServerRpc(int equipo, ServerRpcParams rpcParams = default)
        {          

            if(equipo == -1){

                Position.Value = new Vector3(Random.Range(0.6f, 9f), 1f, Random.Range(-3f, 4f));
                ColorPlayer.Value = listaColores[0];     
                teamPlayer.Value = 0;
            } else if(equipo == 0){
                Position.Value = new Vector3(Random.Range(0.6f, 9f), 1f, Random.Range(-3f, 4f));
                ColorPlayer.Value = listaColores[0];                         
                teamPlayer.Value = 0;                                     
            } else if(equipo == 1){
                Position.Value = new Vector3(Random.Range(10f, 18f), 1f, Random.Range(-3f, 4f));
                ColorPlayer.Value = listaColores[1];
                teamPlayer.Value = 1;
            } else if(equipo == 2){
                Position.Value = new Vector3(Random.Range(-0.5f, -9f), 1f, Random.Range(-3f, 4f));
                ColorPlayer.Value = listaColores[2];                  
                teamPlayer.Value = 2;
            }
        }           
    
        void Update()
        {
            
        }
    }
}