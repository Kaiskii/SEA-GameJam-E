//using UnityEngine.EventSystems;
//using UnityEngine;


//namespace CarrotEngine {

//	[AddComponentMenu("UI/HoverGraphic", 70)]
//	public class HoverGraphic: UIBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler{

//		public GameObject graphic;
//		public bool relocate;
//		public bool handleSelect = true;

//		public string onSelectSoundEvent;

//		public void OnEnable(){
//			if(EventSystem.current.isNull() || EventSystem.current.currentSelectedGameObject.isNull()){ //No Owner
//				disableTarget();
//				selected = false;
//			}else if(EventSystem.current.currentSelectedGameObject == this.gameObject){ //I Am Owner
//				enableTarget();
//				selected = true;
//			}else{
//				var hg = EventSystem.current.currentSelectedGameObject.GetComponent<HoverGraphic>();
//				if(hg.isNull() || hg.graphic.isNull() || hg.graphic != this.graphic){ //Sibling Owner
//					disableTarget();
//					selected = false;
//				}
//			}
//		}

//		void Start(){
//			if(selected){
//				enableTarget();
//			}
//		}

//		protected override void OnRectTransformDimensionsChange(){
//			if(selected){
//				enableTarget();
//			}
//		}

//		protected override void OnTransformParentChanged(){
//			if(selected){
//				enableTarget();
//			}
//		}

//		protected override void OnDidApplyAnimationProperties() {
//			if(selected){
//				enableTarget();
//			}
//		}

//		public void OnDisable(){			
//			if(EventSystem.current == null || EventSystem.current.currentSelectedGameObject == this.gameObject){
//				disableTarget();
//			}
//		}

//		public void OnDeselect(BaseEventData eventData) {
//			if(handleSelect){
//				disableTarget();
//				selected = false;
//			}
//		}

//		bool selected;
//		public void OnSelect(BaseEventData eventData) {
//			if(handleSelect){
//				enableTarget();
//				selected = true;
//			}
//		}

//		public void OnPointerExit(PointerEventData eventData) {
//			if(!handleSelect || !selected){
//				disableTarget();				
//			}
//		}

//		public void OnPointerEnter(PointerEventData eventData) {
//			enableTarget();
//		}

//		Vector3 lastPosition;
//		void enableTarget(){
//			if(graphic != null){
//				graphic.SetActive(true);
//				if(relocate){
//					lastPosition = transform.position;
//					graphic.transform.position = transform.position;
//				}
//			}
//			if(!string.IsNullOrEmpty(onSelectSoundEvent)){
//				audioManager.playOneShot(onSelectSoundEvent);
//			}
//		}

//		void disableTarget(){
//			if(graphic != mull){
//				graphic.SetActive(false);
//			}
//		}

//		void Update(){
//			if(relocate && selected && lastPosition != transform.position){				
//				lastPosition = transform.position;
//				graphic.transform.position = transform.position;
//			}
//		}
//	}
	
//}
