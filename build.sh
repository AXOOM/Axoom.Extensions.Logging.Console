#!/bin/sh
set -e
cd `dirname $0`

src/build.sh $1
src/test.sh
