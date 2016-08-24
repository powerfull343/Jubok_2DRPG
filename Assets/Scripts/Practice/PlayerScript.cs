using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    private Rigidbody2D _PlayerRigid;
    private Animator _PlayerAnim;
    private float _Speed = 5f;
    private bool _facingright = true;
    private float floorYvalue = 0f;
    private bool _jumping = false;

	// Use this for initialization
	void Start () {
        _PlayerRigid = GetComponent<Rigidbody2D>();
        _PlayerAnim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        float Dir = Input.GetAxisRaw("Horizontal");
        _PlayerAnim.SetFloat("speed", Mathf.Abs(Dir));

        MovingCheck(Dir);
        JumpingCheck();       
    }

    void FixedUpdate()
    {

    }

    void MovingCheck(float Dir)
    {
        if (_facingright == true && Dir < 0)
        {
            _facingright = false;
            transform.rotation = Quaternion.Euler(
                new Vector3(transform.rotation.x, 180f, transform.rotation.z));
        }
        else if (_facingright == false && Dir > 0)
        {
            _facingright = true;
            transform.rotation = Quaternion.Euler(
                new Vector3(transform.rotation.x, 0f, transform.rotation.z));
        }

        _PlayerRigid.velocity = new Vector2(Dir * _Speed, _PlayerRigid.velocity.y);
    }

    void JumpingCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _jumping == false)
        {
            _jumping = true;
            float fX = _PlayerRigid.velocity.x;
            _PlayerAnim.SetBool("jump", true);
            //_PlayerRigid.velocity = new Vector2(fX, 10);                      
            _PlayerRigid.AddForce(Vector2.up * 500f);
        }      
    }

    void JumpingReset()
    {
        Debug.Log("false");
        _PlayerAnim.SetBool("jump", false);
        _jumping = false;
    }   
}
