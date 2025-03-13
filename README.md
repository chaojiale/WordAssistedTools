# WordAssistedTools
## 简单介绍
本插件包含一些有趣的Word小工具，如规划Pre时间、提取Word中图片的原图、便捷的API翻译和GPT for Word。  
部分灵感来自于[zotero-pdf-translate](https://github.com/windingwind/zotero-pdf-translate)和[zotero-gpt](https://github.com/MuiseDestiny/zotero-gpt)。

## 项目构建
### 插件
如需自行构建本插件，你需要在Visual Studio安装包中勾选`Office/SharePoint开发`模块。
如果只想自己开发翻译插件，你需要在你的项目中引用`WordAssistedTools.SDK.dll`，详细事宜请点击本插件面板的`开发指南`按钮。

### 安装包
本项目只提供安装包的主要代码，修改自[VstoAddinInstaller](https://github.com/bovender/VstoAddinInstaller)。如需了解安装包详细事宜，请参阅其仓库。  
简单来说，如果你想自己构建安装包，你需要下载`InnoSetup Download Plugin`，并将安装后的`source`目录拷贝到本项目的`deploy`文件夹中，并重命名为`InnoDownloadPlugin`。  

## 注意事项
1.可视化设计器只支持一个Word实例，如果有多个打开的Word窗口，可能会有严重错误。

——le~  
由于时间有限，目前版本没有详细测试所有功能，欢迎反馈bug。  
希望这对你有用:)
