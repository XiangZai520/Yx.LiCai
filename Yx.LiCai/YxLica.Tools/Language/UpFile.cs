using System;

namespace YxLiCai.Tools.Language
{
    /// <summary>
    /// �ϴ��ļ������������ĺ�Ӣ�ĵĴ�����Ϣ��
    /// </summary>
    public class UpFile
    {
        /// <summary>
        /// �ϴ����ļ�̫��
        /// </summary>
        public string Error_FileLength;
        /// <summary>
        /// �ϴ����ļ���ʽ����ȷ
        /// </summary>
        public string Error_FileFormat;
        /// <summary>
        /// ϵͳ����
        /// </summary>
        public string Error_System;
        /// <summary>
        /// �ļ��ϴ��ɹ�
        /// </summary>
        public string Error_Succeed;
        /// <summary>
        /// û��ѡ���ϴ����ļ�����ѡ����ļ�·������ȷ
        /// </summary>
        public string Error_NoFile;

        /// <summary>
        /// ���췽��
        /// </summary>
        /// <param name="Att">1Ϊ���ģ�2ΪӢ��</param>
        public UpFile(int Att)
        {
            if (Att == 1)
            {
                this.set_Cn();
            }
            else
            {
                this.set_En();
            }
        }
        /// <summary>
        /// ���췽�������ش�����ϢΪ����
        /// </summary>
        public UpFile()
        {
            this.set_Cn();
        }

        private void set_Cn()
        {
            this.Error_FileLength = "�ϴ����ļ�̫��";
            this.Error_FileFormat = "�ϴ����ļ���ʽ����ȷ";
            this.Error_System = "ϵͳ����";
            this.Error_Succeed = "�ļ��ϴ��ɹ�";
            this.Error_NoFile = "û��ѡ���ϴ����ļ�����ѡ����ļ�·������ȷ";
        }
        private void set_En()
        {
            this.Error_FileLength = "�ϴ����ļ�̫��";
            this.Error_FileFormat = "�ϴ����ļ���ʽ����ȷ";
            this.Error_System = "ϵͳ����";
            this.Error_Succeed = "�ļ��ϴ��ɹ�";
            this.Error_NoFile = "û��ѡ���ϴ����ļ�����ѡ����ļ�·������ȷ";
        }
    }
}