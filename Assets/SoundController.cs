using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviourSingleton<SoundController>
{

	public List<InAudioNode> Nodes;
	public Dictionary<string, InAudioNode> Sounds;

    // Start is called before the first frame update
    void Start()
    {

		Sounds = new Dictionary<string, InAudioNode> ();

		foreach (InAudioNode node in Nodes) {

			Sounds.Add (node.Name, node);
		}
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public void StopSound (string name) {

		InAudio.StopAllOfNode (Sounds [name]);
	}

	public void PlaySound (string name) {


		InAudio.Play (Camera.main.gameObject, Sounds [name]);

		

	}
}
