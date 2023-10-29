<a name="readme-top"></a>

<div align="center">

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]

</div>



<br />
<div align="center">
  <a href="https://github.com/Minty-Labs/WindowsXSO">
    <img src="Resources/XSOWin_Icon.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">WindowsXSO</h3>

  <p align="center">
    Windows to XSOverlay Notification Relay
    <br />
    <br />
    <a href="https://github.com/Minty-Labs/WindowsXSO/issues">Report Bug</a>
    ·
    <a href="https://github.com/Minty-Labs/WindowsXSO/issues">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
      </ul>
    </li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

Do you always miss notifactions while you're in VR? And, do you use [XSOverlay][XSOverlaySteam]? You can use this program. This program will translate windows notifications into a popup window with XSOverlay while you're in VR.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With

[![DotNet][CSharp]][DotNetUrl]<br>
[![Rider][Rider]][RiderUrl]<br>
[![Obsidian][Obsidian]][ObsidianUrl]

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

Download the [latest version][releases-url] of WindowsXSO.

### Prerequisites

This program requires very specific things to run properly:
* [DotNet Desktop Runtime 7.0.x][DotNetUrl] (For the application)
* Windows 11 Home/Pro
* * Or Windows 10 Home/Pro (Build [22621.1265](https://support.microsoft.com/en-us/topic/february-14-2023-kb5022845-os-build-22621-1265-90a807f4-d2e8-486e-8a43-d09e66319f38)+)
* Granted access to notifications
* * Windows 11
* * * Settings > Privacy & Security > Notifications (Section) > Allow apps to access notifications > ON (true)
* * Windows 10
* * * Settings > Notifications & actions > Get notifications from app and other senders > ON (true)
* * Both
* * * Make sure Focus Assist is OFF (false)



<!-- ROADMAP -->
## Roadmap

- [x] Add auto-update
  - Will toggle-able with config
- [ ] Add Pre-start commands
    - [ ] Configuration control
    - [ ] Run in background (no console window)
- [ ] Multi-language Support
    - [x] English
    - [ ] Spanish
    - [ ] French
    - [ ] Russian
    - [ ] German

See the [open issues][issues-url] for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See [`LICENSE`][license-url] for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Lily - [@MintLiIy](https://x.com/MintLiIy) - contact@mintylabs.dev

Project Link: [https://github.com/Minty-Labs/WindowsXSO](https://github.com/Minty-Labs/WindowsXSO)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* [README Style](https://github.com/othneildrew/Best-README-Template)
* Katie - For help with the Windows API
* [Natsumi](https://github.com/Natsumi-sama) - For help with Updater logic

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
[contributors-shield]: https://img.shields.io/github/contributors/Minty-Labs/WindowsXSO.svg?style=for-the-badge
[contributors-url]: https://github.com/Minty-Labs/WindowsXSO/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/Minty-Labs/WindowsXSO.svg?style=for-the-badge
[forks-url]: https://github.com/Minty-Labs/WindowsXSO/network/members
[stars-shield]: https://img.shields.io/github/stars/Minty-Labs/WindowsXSO.svg?style=for-the-badge
[stars-url]: https://github.com/Minty-Labs/WindowsXSO/stargazers
[issues-shield]: https://img.shields.io/github/issues/Minty-Labs/WindowsXSO.svg?style=for-the-badge
[issues-url]: https://github.com/Minty-Labs/WindowsXSO/issues
[license-shield]: https://img.shields.io/github/issues/Minty-Labs/WindowsXSO.svg?style=for-the-badge
[license-url]: https://github.com/Minty-Labs/WindowsXSO/blob/main/LICENSE
[releases-url]: https://github.com/Minty-Labs/WindowsXSO/releases

[Rider]: https://img.shields.io/badge/Rider-000000?style=for-the-badge&logo=rider&logoColor=white
[RiderUrl]: https://jb.gg/OpenSourceSupport
[CSharp]: https://img.shields.io/badge/DotNet%207-512BD4?style=for-the-badge&logo=csharp&logoColor=white
[DotNetUrl]: https://dotnet.microsoft.com/en-us/download/dotnet/7.0
[Obsidian]: https://img.shields.io/badge/Obsidian-7C3AED?style=for-the-badge&logo=obsidian&logoColor=white
[ObsidianUrl]: https://obsidian.md/
[XSOverlaySteam]: https://store.steampowered.com/app/1173510/XSOverlay/
