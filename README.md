# FontJsonGenerator
简化Minecraft中字体Json配置的创建

# 使用
```Bash
dotnet run -c Release --input <配置文件地址>
```

或

```Bash
dotnet build -c Release
cd bin/Release/net6.0
./FontJsonGenerator --input <配置文件地址>
```

如果要输出结果到文件，可以在后面添加`--output <目标地址>`

例如：
1. 通过`dotnet run -c Release --input examplesettings.json --output <目标资源包的assets/minecraft/font/default.json地址>`生成Json文件
2. 在游戏内加载/重载资源包
3. 在游戏内输入一些资源包范围内的字体（examplesettings中是U+E300~U+E471）
3. 查看生成结果！ ![窗口截图](screenshots/1.png)