#!/usr/bin/env sh

for d in `realpath /dev/disk/by-partuuid/*`
do
  echo $(grep $d /proc/mounts | cut -d ' ' -f 2)
done