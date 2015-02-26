A.将所有项目的运行平台改为需要的,e.g.使用平台是framework3.5就改为3.5

B.如果使用平台是framework3.5,则将LIB\framework3.5中的所需的dll复制至EG.Utility.AppCommon\Lib
  如果使用平台是framework4.0,则将LIB\framework4.0中的所需的dll复制至EG.Utility.AppCommon\Lib

C.需要在有#define语句的文件中改正版本号,如framework3.5平台中则为#define net35
  framework4.0平台则为#define net40
  具体文件有:
  1.EGCommon\DBcommon\dao\AOP.cs
  2.EGCommon\AppCommon\ConfigCache.cs

D.debug后即可得到EGCommon.dll

Ps:
1.复制前dll前请先清空EG.Utility.AppCommon\Lib中的dll,避免版本混乱
1.如果需要添加dll,请找到3.5、4.0的版本,分别将其拷贝至LIB\framework3.5,LIB\framework4.0文件夹上,如果找到其他的版本,最好拷贝至LIB\other文件夹上备份
2.如果需要在cs中添加#define net语句,请在本txt中添加说明

感谢各位同僚的谅解 -by edgar 2013-10-16