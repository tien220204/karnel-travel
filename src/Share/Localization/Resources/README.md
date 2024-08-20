# How to use
- Add or update new Resource from Resources.txt
- Open Visual Studio xxxx Developer Command Prompt 
```bash
Tools --> Command Line --> Developer Command Prompt

or 

Search 'Developer Command Prompt' in Search bar
```

- Run this
```bash
Step 1: cd src/Share
Step 2: resgen Localization\Resources\Resources.txt Localization\Resources\Resources.resx /publicClass /str:cs,KarnelTravel.Share.Localization
```

- Verify your updated in Resources.cs