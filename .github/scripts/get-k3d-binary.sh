#!/bin/bash
set -e

get() {
  local url=$1
  local binary=$2
  local target_dir=$3
  local target_name=$4
  local isTar=$5

  # check if tar
  if [ "$isTar" = true ]; then
    curl -LJ "$url" | tar xvz -C "$target_dir" "$binary"
    mv "$target_dir/$binary" "${target_dir}/$target_name"
  elif [ "$isTar" = false ]; then
    curl -LJ "$url" -o "$target_dir/$target_name"
  fi
  chmod +x "$target_dir/$target_name"
}

#!/bin/bash
set -e

get() {
  local url=$1
  local binary=$2
  local target_dir=$3
  local target_name=$4
  local isTar=$5

  if [ "$isTar" = true ]; then
    curl -LJ "$url" | tar xvz -C "$target_dir" "$binary"
    mv "$target_dir/$binary" "${target_dir}/$target_name"
  elif [ "$isTar" = false ]; then
    curl -LJ "$url" -o "$target_dir/$target_name"
  fi
  chmod +x "$target_dir/$target_name"
}

get "https://getbin.io/k3d-io/k3d?os=darwin&arch=amd64" "k3d" "Devantler.K3dCLI/runtimes/osx-x64/native" "k3d-osx-x64" false
get "https://getbin.io/k3d-io/k3d?os=darwin&arch=arm64" "k3d" "Devantler.K3dCLI/runtimes/osx-arm64/native" "k3d-osx-arm64" false
get "https://getbin.io/k3d-io/k3d?os=linux&arch=amd64" "k3d" "Devantler.K3dCLI/runtimes/linux-x64/native" "k3d-linux-x64" false
get "https://getbin.io/k3d-io/k3d?os=linux&arch=arm64" "k3d" "Devantler.K3dCLI/runtimes/linux-arm64/native" "k3d-linux-arm64" false
curl -s "https://api.github.com/repos/k3d-io/k3d/releases" | grep "browser_download.*windows-amd64.exe" | cut -d '"' -f 4 | sort -V | tail -n 1 | xargs curl -LJ -o Devantler.K3dCLI/runtimes/win-x64/native/k3d-win-x64.exe
