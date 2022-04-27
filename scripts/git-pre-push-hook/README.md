# Git pre-push script

In this folder, there is a `pre-push.sh` bash script file. This script runs `dotnet test` before you push to the remote repository, potentially saving yourself a broken build.

## Activation

To use it, copy the `pre-push.sh` script in the root of the repo to the `.git/hooks` folder (if you can't see a `.git` folder, that's because your OS is hiding hidden files. If on Windows, set Windows explorer to "show hidden folders") and rename the script to `pre-push`.

Or you could **run the following commands FROM THIS FOLDER**:

Powershell:

```powershell
Copy-Item pre-push.sh -Destination "../../.git/hooks/pre-push" -Force
```

Bash:

```bash
cp pre-push.sh ../../.git/hooks/pre-push
```

## De-activation

Either remove `pre-push` from the `.git/hooks` folder, or **run the following commands FROM THIS FOLDER**:

Powershell:

```powershell
Remove-Item "../../.git/hooks/pre-push"
```

Bash:

```bash
rm ../../.git/hooks/pre-push
```