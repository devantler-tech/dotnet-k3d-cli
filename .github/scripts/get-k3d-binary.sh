#!/bin/bash
set -e

get() {
  local url=$1
  local binary=$2
  local target_dir=$3
  local target_name=$4
  local isTar=$5

  echo "Downloading $target_name from $url"
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
get "https://getbin.io/k3d-io/k3d?os=windows&arch=amd64" "k3d.exe" "Devantler.K3dCLI/runtimes/win-x64/native" "k3d-win-x64.exe" false
