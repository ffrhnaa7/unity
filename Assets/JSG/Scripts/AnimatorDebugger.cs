using UnityEngine;

public class AnimatorDebugger : MonoBehaviour
{
    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anim == null) return;

        // ���� ��� ���� �ִϸ��̼� ������ ��������
        //AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        //if (clipInfo.Length > 0)
        {
            //Debug.Log("���� ��� ���� �ִϸ��̼�: " + clipInfo[0].clip.name);
        }
    }
}
