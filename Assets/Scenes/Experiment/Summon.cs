using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Camera.main �Ń��C���J�����iMainCamera �^�O�̕t���� Camera�j���擾����
            // Camera.ScreenToWorldPoint �֐��ŁA�X�N���[�����W�����[���h���W�ɕϊ�����
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;    // Z ���W���J�����Ɠ����ɂȂ��Ă���̂ŁA���Z�b�g����
            this.transform.position = mousePosition;

        }
    }
}
