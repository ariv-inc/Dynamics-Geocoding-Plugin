
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<br />
<p align="center">
  <h3 align="center">Dynamics-Geocoding-Plugin</h3>

  <p align="center">
    Microsoft Dynamics Pluging that uses Bing maps to geocode and populate longitude and latitude fields
    <br />
    <br />
    <a href="https://github.com/ariv-inc/Dynamics-Geocoding-Plugin/issues">Report Bug</a>
    ·
    <a href="https://github.com/ariv-inc/Dynamics-Geocoding-Plugin/issues">Request Feature</a>
  </p>
</p>

## About The Project
Dynamics comes with the built in fields *Latitude* and *Longitude*. Unfortunatly, these fields are not populated. This is why we created this plugin that populates these fields with the address provided.

## Prerequisites
Before you get started, you will need the following:

 - Bing maps API key

	You can get a Bing Maps key by going to the Bing Maps Dev Center:
	
	https://www.bingmapsportal.com/
	
 - Plug-in Registration tool
 
	In order to register a plugin into Microsoft Dynamics, you will need the Plug-in Registration Tool
	Here is the documentation on how to get the tool: 
	
	https://docs.microsoft.com/en-us/powerapps/developer/data-platform/download-tools-nuget

 - Ariv Dynamics Geocoding Plugin

 	You can download the latest release [here](https://github.com/ariv-inc/Dynamics-Geocoding-Plugin/releases/latest)
	
###

## Getting Started
### Connect using the Plug-in Registration tool

1.  After you have downloaded the Plug-in registration tool, click the  `PluginRegistration.exe`  to open it.
    
2.  Click  **Create new Connection**  to connect to your instance.
    
3.  Make sure  **Office 365**  is selected. If you are connecting using a Microsoft account other than one you are currently using, click  **Show Advanced**.

    ![Logging in with the Plug-in registration tool](https://docs.microsoft.com/en-us/powerapps/developer/data-platform/media/tutorial-write-plug-in-prt-login.png)
    
4.  Enter your credentials and click  **Login**.
    
5.  If your Microsoft Account provides access to multiple environments, you will need to choose an environment.
    
6.  After you are connected, you will see any existing registered plug-ins & custom workflow activities
    
   ### Register the assembly

1.  In the  **Register**  drop-down, select  **New Assembly**. 
    
    ![Register new assembly][register-new-assembly]
    
2.  In the  **Register New Assembly**  dialog, select the ellipses (**…**) button and browse to the assembly *Ariv.Dynamics.BingGeocoding.Plugin.dll*  
    
    ![Register new assembly dialog][register-new-assembly2]
    
3.  Click  **Register Selected Plug-ins**.
    
4.  You will see a  **Registered Plug-ins**  confirmation dialog.
     
    ![Register new assembly dialog][register-new-assembly-confirmation]
     
5.  Click  **OK**  to close the dialog and close the  **Register New Assembly**  dialog.

### Register a new step (Create)

1.  Right-click the  **(Assembly) Ariv.Dynamics.BingGeocoding.Plugin**  and select  **Register New Step**.
    
    ![Register a new step][register-new-step]
    
2.  In the  **Register New Step**  dialog, set the following fields:
       
    | Setting | Value |
    |--|--|
    | Message | Create |
    | Primary Entity| account |
    | Event Pipeline Stage of Execution | PostOperation |
    | Execution Mode | Asynchronous |
    
 3. In the  **Secure Configuration** field, set the following with your own Bing Maps key:

    `{"key":"YourBingKey"}`

4.  Click  **Register New Step**  to complete the registration and close the  **Register New Step**  dialog.

    ![Register new step create][register-new-step-create]

### Register a new step (Update)

1.  Right-click the  **(Assembly) Ariv.Dynamics.BingGeocoding.Plugin**  and select  **Register New Step**.   
    
    ![Register a new step][register-new-step]
    
2.  In the  **Register New Step**  dialog, set the following fields:
       
    | Setting | Value |
    |--|--|
    | Message | Update |
    | Primary Entity| account |
    | Filtering Attributes | address1_composite |
    | Event Pipeline Stage of Execution | PostOperation |
    | Execution Mode | Asynchronous |
    
3.  In the  **Secure Configuration** field, set the following with your own Bing Maps key:

    `{"key":"YourBingKey"}`

4.  Click  **Register New Step**  to complete the registration and close the  **Register New Step**  dialog.

    ![Register new step create][register-new-step-update]

### Repeat for each entities
You can repeate the previous steps, ***Register a new step (Create)*** and ***Register a new step (Update)*** for the following entities:
 - account
 - competitor
 - contact
 - lead

## License

Distributed under the MIT License. See `LICENSE` for more information.

[contributors-shield]: https://img.shields.io/github/contributors/ariv-inc/Dynamics-Geocoding-Plugin.svg?style=for-the-badge
[contributors-url]: https://github.com/ariv-inc/Dynamics-Geocoding-Plugin/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/ariv-inc/Dynamics-Geocoding-Plugin.svg?style=for-the-badge
[forks-url]: https://github.com/ariv-inc/Dynamics-Geocoding-Plugin/network/members
[stars-shield]: https://img.shields.io/github/stars/ariv-inc/Dynamics-Geocoding-Plugin.svg?style=for-the-badge
[stars-url]: https://github.com/ariv-inc/Dynamics-Geocoding-Plugin/stargazers
[issues-shield]: https://img.shields.io/github/issues/ariv-inc/Dynamics-Geocoding-Plugin.svg?style=for-the-badge
[issues-url]: https://github.com/ariv-inc/Dynamics-Geocoding-Plugin/issues
[license-shield]: https://img.shields.io/github/license/ariv-inc/Dynamics-Geocoding-Plugin?style=for-the-badge
[license-url]: https://github.com/ariv-inc/Dynamics-Geocoding-Plugin/blob/main/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/company/ariv-inc/

[register-new-assembly]: images/register-new-assembly.png
[register-new-assembly2]: images/register-new-assembly2.png
[register-new-assembly-confirmation]: images/register-new-assembly-confirmation.png
[register-new-step]: images/register-new-step.png
[register-new-step-create]: images/register-new-step-create.png
[register-new-step-update]: images/register-new-step-update.png
