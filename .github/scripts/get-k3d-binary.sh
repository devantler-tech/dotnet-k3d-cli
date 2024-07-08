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

get "https://getbin.io/k3d-io/k3d?os=darwin&arch=amd64" "k3d" "src/Devantler.K3dCLI/assets/binaries" "k3d-darwin-amd64" false
get "https://getbin.io/k3d-io/k3d?os=darwin&arch=arm64" "k3d" "src/Devantler.K3dCLI/assets/binaries" "k3d-darwin-arm64" false
get "https://getbin.io/k3d-io/k3d?os=linux&arch=amd64" "k3d" "src/Devantler.K3dCLI/assets/binaries" "k3d-linux-amd64" false
get "https://getbin.io/k3d-io/k3d?os=linux&arch=arm64" "k3d" "src/Devantler.K3dCLI/assets/binaries" "k3d-linux-arm64" false
