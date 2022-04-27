# When adding a new script:
To enable execution privileges on the script in Linux/Github Actions, use the following command:

```bash
git update-index --chmod=+x <path_to_file>
```

e.g.

```bash
git update-index --chmod=+x ./scripts/dotnet-sca/dotnet-sca.sh
```