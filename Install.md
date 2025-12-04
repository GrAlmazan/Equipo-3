``` bash

titulacion@ubuntu-server:~$ rm actions-runner/ -d -r -f
titulacion@ubuntu-server:~$ ls
Equipo-3
titulacion@ubuntu-server:~$ mkdir actions-runner && cd actions-runner
titulacion@ubuntu-server:~/actions-runner$ curl -o actions-runner-linux-x64-2.329.0.tar.gz -L https://github.com/actions/runner/releases/download/v2.329.0/actions-runner-linux-x64-2.329.0.tar.gz
  % Total    % Received % Xferd  Average Speed   Time    Time     Time  Current
                                 Dload  Upload   Total   Spent    Left  Speed
  0     0    0     0    0     0      0      0 --:--:-- --:--:-- --:--:--     0
100  216M  100  216M    0     0  9043k      0  0:00:24  0:00:24 --:--:-- 11.1M
titulacion@ubuntu-server:~/actions-runner$ echo "194f1e1e4bd02f80b7e9633fc546084d8d4e19f3928a324d512ea53430102e1d  actions-runner-linux-x64-2.329.0.tar.gz" | shasum -a 256 -c
actions-runner-linux-x64-2.329.0.tar.gz: OK
titulacion@ubuntu-server:~/actions-runner$ tar xzf ./actions-runner-linux-x64-2.329.0.tar.gz
titulacion@ubuntu-server:~/actions-runner$ ./config.sh --url https://github.com/GrAlmazan/Equipo-3 --token BB7QJKLHWHNIIXNLYU5UJPDJFNLB4

--------------------------------------------------------------------------------
|        ____ _ _   _   _       _          _        _   _                      |
|       / ___(_) |_| | | |_   _| |__      / \   ___| |_(_) ___  _ __  ___      |
|      | |  _| | __| |_| | | | | '_ \    / _ \ / __| __| |/ _ \| '_ \/ __|     |
|      | |_| | | |_|  _  | |_| | |_) |  / ___ \ (__| |_| | (_) | | | \__ \     |
|       \____|_|\__|_| |_|\__,_|_.__/  /_/   \_\___|\__|_|\___/|_| |_|___/     |
|                                                                              |
|                       Self-hosted runner registration                        |
|                                                                              |
--------------------------------------------------------------------------------

# Authentication


√ Connected to GitHub

# Runner Registration

Enter the name of the runner group to add this runner to: [press Enter for Default]

Enter the name of runner: [press Enter for ubuntu-server]

This runner will have the following labels: 'self-hosted', 'Linux', 'X64'
Enter any additional labels (ex. label-1,label-2): [press Enter to skip]

√ Runner successfully added

# Runner settings

Enter name of work folder: [press Enter for _work]

√ Settings Saved.

titulacion@ubuntu-server:~/actions-runner$ sudo ./svc.sh install
Creating launch runner in /etc/systemd/system/actions.runner.GrAlmazan-Equipo-3.ubuntu-server.service
Run as user: titulacion
Run as uid: 1000
gid: 1000
Created symlink /etc/systemd/system/multi-user.target.wants/actions.runner.GrAlmazan-Equipo-3.ubuntu-server.service → /etc/systemd/system/actions.runner.GrAlmazan-Equipo-3.ubuntu-server.service.
titulacion@ubuntu-server:~/actions-runner$ sudo ./svc.sh start

/etc/systemd/system/actions.runner.GrAlmazan-Equipo-3.ubuntu-server.service
● actions.runner.GrAlmazan-Equipo-3.ubuntu-server.service - GitHub Actions Runner (GrAlmazan-Equipo-3.ubuntu-server)
     Loaded: loaded (/etc/systemd/system/actions.runner.GrAlmazan-Equipo-3.ubuntu-server.service; enabled; preset: enabled)
     Active: active (running) since Sat 2025-11-29 19:24:23 UTC; 9ms ago
   Main PID: 11052 (runsvc.sh)
      Tasks: 2 (limit: 2267)
     Memory: 1.3M (peak: 1.3M)
        CPU: 4ms
     CGroup: /system.slice/actions.runner.GrAlmazan-Equipo-3.ubuntu-server.service
             ├─11052 /bin/bash /home/titulacion/actions-runner/runsvc.sh
             └─11055 ./externals/node20/bin/node ./bin/RunnerService.js

Nov 29 19:24:23 ubuntu-server systemd[1]: Started actions.runner.GrAlmazan-Equipo-3.ubuntu-serv…rver).
Nov 29 19:24:23 ubuntu-server runsvc.sh[11052]: .path=/usr/local/sbin:/usr/local/bin:/usr/sbin:/…tools
Hint: Some lines were ellipsized, use -l to show in full.
titulacion@ubuntu-server:~/actions-runner$
```