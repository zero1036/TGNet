A.��������Ŀ������ƽ̨��Ϊ��Ҫ��,e.g.ʹ��ƽ̨��framework3.5�͸�Ϊ3.5

B.���ʹ��ƽ̨��framework3.5,��LIB\framework3.5�е������dll������EG.Utility.AppCommon\Lib
  ���ʹ��ƽ̨��framework4.0,��LIB\framework4.0�е������dll������EG.Utility.AppCommon\Lib

C.��Ҫ����#define�����ļ��и����汾��,��framework3.5ƽ̨����Ϊ#define net35
  framework4.0ƽ̨��Ϊ#define net40
  �����ļ���:
  1.EGCommon\DBcommon\dao\AOP.cs
  2.EGCommon\AppCommon\ConfigCache.cs

D.debug�󼴿ɵõ�EGCommon.dll

Ps:
1.����ǰdllǰ�������EG.Utility.AppCommon\Lib�е�dll,����汾����
1.�����Ҫ���dll,���ҵ�3.5��4.0�İ汾,�ֱ��俽����LIB\framework3.5,LIB\framework4.0�ļ�����,����ҵ������İ汾,��ÿ�����LIB\other�ļ����ϱ���
2.�����Ҫ��cs�����#define net���,���ڱ�txt�����˵��

��л��λͬ�ŵ��½� -by edgar 2013-10-16