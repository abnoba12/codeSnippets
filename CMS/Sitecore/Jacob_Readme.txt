-- Things to check --
1. Make sure you have the SVN's web.config

2. Check and make sure your App_Config\connectionStrings.conf is correct.

3. Add the follwing two lines to your Global.asax file in the Application_Start() function:
	System.Security.Cryptography.RSACryptoServiceProvider.UseMachineKeyStore = true;
	System.Security.Cryptography.DSACryptoServiceProvider.UseMachineKeyStore = true;

4. In the web.config find that line that points to the data folder and make sure it is correct for your local machine. <sc.variable name="dataFolder" value="S:\mysitecore\data\" />

-- Notes --
1. The license.xml file is in the data folder (sibling to the website folder).

-- Tips & Tricks --
Find Links - To find what links to and from an item in using the sitecore content editor you go to: Navigate -> Links.

Mass Edits - Go to the Developer tab (If it isn't there right click in the tab area and there will be an option to add it), navigate to the item or tree you want, click serialize item or tree. Edit the file located in "Data/serialize", click update item or tree to send changes back into the system. Once you verify the changes are good click update database. Note: This only appears to work on content and not properties.

-- Common Errors & How to fix --
	-System.Data.SQLite - Error: "Sitecore Could not load file or assembly ‘System.Data.SQLite" Solution: Delete the System.Data.SQLite from the website\bin folder. 

	-It is possible that .NET performance counters are not loaded correctly, so try to reload them:
	  1. Start an administrator command prompt
	  2. Run unlodctr .NETFramework
	  3. Run lodctr %WINDIR%\Microsoft.NET\Framework\<Framework_ver>\CORPerfMonSymbols.ini 
	  C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\CORPerfMonSymbols.ini
	where <Framework_ver> is the directory of the .NET Framework version which contains the CORPerfMonSymbols.ini file. It should be either v4.0.xxx or v2.0.xxx if 4.0 is not installed.
		If that doesn't work then try this command lodctr /R
	  4. Restart IIS.
