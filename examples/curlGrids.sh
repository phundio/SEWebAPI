#!/bin/bash

urls=`cat gridUrls.conf | egrep -v '^\#' | egrep -v '^\s*$'`

for i in $urls; do
  echo $i;
  curl $i/grid
done
