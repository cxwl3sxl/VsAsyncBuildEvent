# VsAsyncBuildEvent
用于为Visual Studio 添加异步编译事件的一个工具。

目的：由于在Visual Studio编译完成事件中增加了自动的文档生成操作，即调用了Sandcastle Help File Builder来生成帮助文件，导致VS编译时间过长，因此开发了本工具用于异步调用相关编译命令，用于提高编译速度。

界面截图：

![](https://github.com/cxwl3sxl/VsAsyncBuildEvent/blob/master/%E6%8D%95%E8%8E%B7.PNG)

![](https://github.com/cxwl3sxl/VsAsyncBuildEvent/blob/master/1.PNG)

![](https://github.com/cxwl3sxl/VsAsyncBuildEvent/blob/master/2.PNG)

使用方法：
VsAsyncBuildEvent.exe msbuild.exe $(TargetDir)demo.shfbproj
表示：在编译完成之后，使用调用msbuild.exe程序，并且传递$(TargetDir)demo.shfbproj作为其参数。