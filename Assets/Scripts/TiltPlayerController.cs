using UnityEngine;
  using UnityEngine.UI;

  public class TiltPlayerController : MonoBehaviour
  {
      public Transform tiltController;
      public float maxTiltDegrees = 35f;
      public float deadZoneDegrees = 5f;
      public float speed = 4f;
      public float rotationSpeed = 720f;
      public Text countText;
      public Text winText;

      private Rigidbody rb;
      private Quaternion controllerRestRotation;
      private int count;

      void Start()
      {
          rb = GetComponent<Rigidbody>();
          if (tiltController != null)
              controllerRestRotation = tiltController.rotation;
          count = 0;
          SetCountText();
          if (winText != null) winText.text = "";
      }

      void FixedUpdate()
      {
          Vector3 movement = Vector3.zero;
          if (tiltController != null)
          {
              Quaternion delta = Quaternion.Inverse(controllerRestRotation) * tiltController.rotation;
              Vector3 euler = delta.eulerAngles;
              float pitch = Mathf.DeltaAngle(0f, euler.x);
              float roll = Mathf.DeltaAngle(0f, euler.z);
              float h = Mathf.Abs(roll) < deadZoneDegrees ? 0f : Mathf.Clamp(roll / maxTiltDegrees, -1f, 1f);
              float v = Mathf.Abs(pitch) <deadZoneDegrees ? 0f : Mathf.Clamp(pitch / maxTiltDegrees,-1f, 1f);
              movement = new Vector3(-h, 0f, v);
          }

          Vector3 velocity = movement * speed;
          rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

          if (movement.sqrMagnitude > 0.01f)
          {
              Quaternion target = Quaternion.LookRotation(movement);
              rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, target, rotationSpeed *Time.fixedDeltaTime));
          }
      }

      public void RecalibrateRest()
      {
          if (tiltController != null)
              controllerRestRotation = tiltController.rotation;
      }

      void OnTriggerEnter(Collider other)
      {
          if (other.gameObject.CompareTag("Pick Up"))
          {
              other.gameObject.SetActive(false);
              count++;
              SetCountText();
          }
      }

      void SetCountText()
      {
          if (countText != null) countText.text = "Count: " + count;
          if (count >= 8 && winText != null) winText.text = "You Win!";
      }
  }