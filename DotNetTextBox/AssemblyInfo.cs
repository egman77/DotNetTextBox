using System.Reflection;
using System.Web.UI;
using System.Runtime.CompilerServices;
using System.Resources;
//
// �йس��򼯵ĳ�����Ϣ��ͨ������ 
//���Լ����Ƶġ�������Щ����ֵ���޸������
//��������Ϣ��
//
[assembly: AssemblyTitle("DotNetTextBox Server Control")]
[assembly: AssemblyDescription("AspxCn.Com.Cn Crew")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("DotNet�л���")]
[assembly: AssemblyProduct("DotNetTextBox V6.0.9 Retail Version")]
[assembly: AssemblyCopyright("Aspxcn.Com.Cn�л�����Ȩ����")]
[assembly: AssemblyTrademark("DotNetTextBox")]
[assembly: AssemblyCulture("")]
[assembly: TagPrefixAttribute("DotNetTextBox", "DNTB")]
//
// ���򼯵İ汾��Ϣ�������� 4 ��ֵ��
//
//      ���汾
//      �ΰ汾
//      �ڲ��汾��
//      �޶���
//
// ������ָ������ֵ����ʹ�á��޶��š��͡��ڲ��汾�š���Ĭ��ֵ������Ϊ�����·�ʽ 
// ʹ�á�*����

//[assembly: AssemblyVersion("6.0.8")]
[assembly: AssemblyVersion("6.0.9")] //hdh ����

//
// Ҫ�Գ��򼯽���ǩ��������ָ��Ҫʹ�õ���Կ���йس���ǩ���ĸ�����Ϣ����ο� 
// Microsoft .NET ����ĵ���
//
// ʹ����������Կ�������ǩ������Կ��
//
// ע�⣺
//   (*) ���δָ����Կ������򼯲��ᱻǩ����
//   (*) KeyName ��ָ�Ѿ���װ��
//       ������ϵļ��ܷ����ṩ���� (CSP) �е���Կ��KeyFile ��ָ����
//       ��Կ���ļ���
//   (*) ��� KeyFile �� KeyName ֵ����ָ������ 
//       ��������Ĵ���
//       (1) ����� CSP �п����ҵ� KeyName����ʹ�ø���Կ��
//       (2) ��� KeyName �����ڶ� KeyFile ���ڣ��� 
//           KeyFile �е���Կ��װ�� CSP �в���ʹ�ø���Կ��
//   (*) Ҫ���� KeyFile������ʹ�� sn.exe��ǿ���ƣ�ʵ�ù��ߡ�
//        ��ָ�� KeyFile ʱ��KeyFile ��λ��Ӧ��
//        ����ڡ���Ŀ���Ŀ¼������Ŀ���
//        Ŀ¼��λ��ȡ����������ʹ�ñ�����Ŀ���� Web ��Ŀ��
//        ���ڱ�����Ŀ����Ŀ���Ŀ¼����Ϊ
//       <Project Directory>\obj\<Configuration>�����磬��� KeyFile λ�ڸ�
//       ��ĿĿ¼�У�Ӧ�� AssemblyKeyFile 
//       ����ָ��Ϊ [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//        ���� Web ��Ŀ����Ŀ���Ŀ¼����Ϊ
//       %HOMEPATH%\VSWebCache\<Machine Name>\<Project Directory>\obj\<Configuration>��
//   (*) ���ӳ�ǩ������һ���߼�ѡ�� - �й����ĸ�����Ϣ������� Microsoft .NET ���
//       �ĵ���
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyFileVersionAttribute("6.0.9")]
[assembly: NeutralResourcesLanguageAttribute("zh-CHS")]
