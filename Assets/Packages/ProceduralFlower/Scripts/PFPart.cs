using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mattatz.ProceduralFlower {

    [System.Serializable]
    public enum PFPartType {
        None,
        Petal,
        Stover
    };

	public class PFPart : MonoBehaviour {

		const string PROPERTY_COLOR = "_Color2";
		const string PROPERTY_BEND = "_Bend";
		const string PROPERTY_T = "_T";

        public PFPartType Type {
            get {
                return type;
            }
        }

		MeshRenderer rnd {
			get {
				if(_renderer == null) {
					_renderer = GetComponent<MeshRenderer>();
				}
				return _renderer;
			}
		}

		MaterialPropertyBlock block {
			get {
				if(_block == null) {
					_block = new MaterialPropertyBlock();
					rnd.GetPropertyBlock(_block);
				}
				return _block;
			}
		}

		MaterialPropertyBlock _block;
		MeshRenderer _renderer;
		public List<PFSegment> children = new List<PFSegment>();

		public readonly float EPSILON = 0.1f;
        [SerializeField] PFPartType type = PFPartType.None;
		float multiplySpeed = 1f;
		float speed = 1f;

		bool animating = false;
		float ticker = 0f;

		void Update () {
			if(animating) {
				ticker += Time.deltaTime * multiplySpeed * speed;
				Fade(ticker);
				children.ForEach(anim => anim.Animate(speed, ticker));
				if(ticker > 1f + EPSILON) {
					animating = false;
				}
			}
		}

        public void SetType (PFPartType tp) {
            type = tp;
        }

		public void Colorize (Color color) {
			if(type != PFPartType.None) {
				block.SetColor(PROPERTY_COLOR, color);
				rnd.SetPropertyBlock(block);			
			}
		}

		public void Bend (float bend) {
			if(type != PFPartType.None) {
				block.SetFloat(PROPERTY_BEND, bend);
				rnd.SetPropertyBlock(block);			
			}
		}

		public void Fade (float t) {
			if(type != PFPartType.None) {
				block.SetFloat(PROPERTY_T, t);
				rnd.SetPropertyBlock(block);			
			}
		}

		public void SetSpeed (float m) {
			multiplySpeed = m;
		}

		public void Add (PFPart part, float ratio) {
			children.Add(new PFSegment(part, ratio));
		}

		public void Animate (float s = 1f) {
			speed = s;
			animating = true;
			ticker = 0f;
		}

		[System.Serializable]
		public class PFSegment {
			public readonly PFPart part;
			public readonly float ratio;

			bool animating = false;

			public PFSegment (PFPart p, float r) {
				part = p;
				ratio = r;
			}

			public void Animate (float speed, float r) {
				if(!animating && r <= ratio) {
					part.Animate(speed);
				}
			}

		}

	}
		
}

