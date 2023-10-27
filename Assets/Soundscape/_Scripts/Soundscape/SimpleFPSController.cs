using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Components;

[RequireComponent(typeof(Rigidbody))]
public class SimpleFPSController : NetworkBehaviour {

	public float lookSpeed = 200;
	public float moveSpeed = 10;
	public float moveForce = 10;
	public float jumpForce = 20;
	public float dampening = 2;

	public bool isLocalPlayer = false;

	private Vector3 bottom = new Vector3(0, -1, 0);

	private Camera head;
	private Rigidbody body;

	private float lookPitch;

	public bool texting = false;
	public TMP_InputField inputField;
	public GameObject mallet;
	public GameObject noteField;
	public GameObject c;

	Vector3 initialLoc, prevLoc;
	Quaternion initialRoc, prevRoc;
	bool hasMoved = false;

	int fov = 60;

	public void Awake() {
		head = GetComponentInChildren<Camera>();
		body = GetComponent<Rigidbody>();
		//Cursor.lockState = CursorLockMode.Locked;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		
		isLocalPlayer = IsClient;
		enabled = isLocalPlayer;
		if (!IsOwner)
		{
			this.enabled = false;
			body.useGravity = false;
			Destroy(noteField);
			Destroy(c);
			enabled = false;
			return;
		}

		if (NoteManager.instance != null)
		{
			inputField = NoteManager.instance.gameObject.GetComponentInChildren<TMP_InputField>(true);
		}
		initialLoc = prevLoc = transform.position;
		initialRoc = prevRoc = transform.rotation;
	}

	public void Update() {
		//jump
		if (isLocalPlayer && !texting) {
			if (Input.GetButtonDown("Jump")) {
				body.AddForce(-Physics.gravity.normalized * jumpForce, ForceMode.Impulse);
			}

			if (Input.GetKeyDown(KeyCode.Escape) && !SceneManager.GetActiveScene().name.Contains("Asocial")) {
				Debug.Log("Quiting!");
				Application.Quit();
			}

			if (Input.GetKeyDown(KeyCode.R)) {
				prevLoc = initialLoc;
				transform.position = initialLoc;
				hasMoved = false;
			}

			if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Z)) {
				fov--;
				if (fov < 20) {
					fov = 20;
				} else {
					head.fieldOfView = (float)fov;
				}
			} else if (fov < 60) {
				fov++;
				head.fieldOfView = (float)fov;
			}
		}

		if (isLocalPlayer && Input.GetKeyDown(KeyCode.T)) {
			texting = true;
			inputField.transform.parent.gameObject.SetActive(true);
			inputField.ActivateInputField();
		}

		if (texting) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				inputField.text = "";
				inputField.DeactivateInputField(true);
				inputField.transform.parent.gameObject.SetActive(false);
				texting = false;
			} else if (Input.GetKeyDown(KeyCode.Return)) {
				//create a note
				if (NoteManager.instance != null) {
					NoteManager.instance.PostNote(inputField.text, transform.position);
				} else {
					OfflineNoteManager.instance.PostNote(inputField.text, transform.position);
				}
				inputField.text = "";
				inputField.DeactivateInputField(true);
				inputField.transform.parent.gameObject.SetActive(false);
				texting = false;
			}
		}

	}

	public void FixedUpdate() {
		if (isLocalPlayer && !texting) {
			//look
			var lookDelta = new Vector2(0, 0); //new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * lookSpeed * 0.033f; // * Time.fixedDeltaTime;

			if (Input.GetKey(KeyCode.UpArrow)) {
				lookDelta += new Vector2(0f, 1f);
			}

			if (Input.GetKey(KeyCode.DownArrow)) {
				lookDelta += new Vector2(0f, -1f);
			}

			if (Input.GetKey(KeyCode.LeftArrow)) {
				lookDelta += new Vector2(-1f, 0f);
			}

			if (Input.GetKey(KeyCode.RightArrow)) {
				lookDelta += new Vector2(1f, 0f);
			}

			var deltaYaw = Quaternion.AngleAxis(lookDelta.x, Vector3.up);
			transform.localRotation *= deltaYaw;

			lookPitch += -lookDelta.y;
			lookPitch = Mathf.Clamp(lookPitch, -90, 90);

			head.transform.localRotation = Quaternion.Euler(lookPitch, 0, 0);

			var grounded = Grounded;

			if (grounded) body.drag = dampening;
			else body.drag = 0;

			var horizVel = body.velocity;
			horizVel.y = 0;

			//move
			var moveWish = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
			if (moveWish.magnitude > 1) moveWish = moveWish.normalized;
			if (horizVel.magnitude > moveSpeed && Physics.gravity.y <= -9f) return;

			//local to global
			moveWish = transform.TransformVector(moveWish);
			var force = moveWish * moveForce * Time.fixedDeltaTime;

			if (force.magnitude > 0) {
				if (!grounded) force *= .5f;
				body.AddForce(force, ForceMode.Force);
			}

			if (transform.position == initialLoc && transform.rotation == initialRoc && hasMoved) {
				transform.position = prevLoc;
				transform.rotation = prevRoc;
				ChatManager.instance.AddNote("<color=red>Error</color>: There was some kind of connection issue... don't worry about it!");
			} else if (!hasMoved && transform.position != initialLoc) {
				hasMoved = true;
			}

			prevLoc = transform.position;
			prevRoc = transform.rotation;
		}
	}

	public bool Grounded {
		get {
			if (isLocalPlayer) {
				if (Physics.gravity.y > -9f) {
					return true;
				}

				var hits = Physics.RaycastAll(new Ray(transform.position + bottom + transform.up * .01f, Physics.gravity), .1f);
				for (int i = 0; i < hits.Length; i++) {
					if (hits[i].rigidbody == body) continue;
					return true;
				}
			}
			return false;
		}
	}
}