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

        // 현재 재생 중인 애니메이션 정보를 가져오기
        //AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        //if (clipInfo.Length > 0)
        {
            //Debug.Log("현재 재생 중인 애니메이션: " + clipInfo[0].clip.name);
        }
    }
}
