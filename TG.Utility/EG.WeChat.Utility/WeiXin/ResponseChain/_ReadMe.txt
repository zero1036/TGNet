【ResponseChain】目录，用于 应答链 功能的类。

主要包含：
Enums			枚举值
Handlers		应答的处理器
ResponseConfig	应答链的配置支持
ResponseService	应答链的服务类
Results			应答响应结果
Structs			数据结构


---- 思路备忘 ----
1.ResponseService被创建。（一个微信用户上下文，只对应一个服务实例）；
2.ResponseService读取ResponseConfig的内容，根据其配置的内容，创建填充自己的子服务实例，拥有具体的响应逻辑。
3.交互概况： 微信客户端触发	=>  IIS		=>  微信SDK		=>  ResponseService进行处理，并获得Results结果；
			Results结果		=>  构建出微信SDK的结果对象	=>  IIS = > 返回微信客户端。
4.每一个答应点，理解为一个Node节点。
  节点有两种可以进行应答的状态：“Dealing”和“Done”。
  前者主要进行核心的交互，后者（可选）只用于处理“完成这个功能之后，还需处理的其他简单交互，比如节点跳转”。

