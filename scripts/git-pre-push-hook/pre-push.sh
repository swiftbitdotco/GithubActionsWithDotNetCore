#!/bin/bash
# NOTE: script taken from: https://github.com/Redfern/dot-net-core-pre-push-checks

RED='\033[0;31m'
GREEN='\033[1;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

#Let's speed things up a little bit
DOTNET_CLI_TELEMETRY_OPTOUT=1
DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

# ------------------------------------------------------------
# dotnet list package --vulnerable --include-transitive
# ------------------------------------------------------------
dotnet restore # required first

dotnet list package --vulnerable --include-transitive | tee results.log

GREP_VULN=`grep -c 'has the following vulnerable packages' results.log`
GREP_CRIT=`grep -c 'Critical' results.log`
GREP_HIGH=`grep -c 'High' results.log`

if [[ "$GREP_VULN" != "0" ]]
then
  if [ "$GREP_CRIT" == "0" -a "$GREP_HIGH" == "0" ]
  then
    echo "### Vulnerable packages found - not high or critical ###"
  fi
  
  if [ "$GREP_CRIT" == "1" -a "$GREP_HIGH" == "1" ]
  then
    echo "### High/critical vulnerable packages found ###"
	exit 1
  fi  
fi

echo "### No vulnerable packages found ###"

# ------------------------------
# build the project
# ------------------------------
echo ""
echo -e "${YELLOW}############${NC}"
echo -e "${YELLOW}  Building  ${NC}"
echo -e "${YELLOW}############${NC}"
echo ""
dotnet build
rc=$?
# $? is a shell variable which stores the return code from what we just ran
if [[ $rc != 0 ]] ; then
    # A non-zero return code means an error occurred, so tell the user and exit
    echo -e "${RED}Build failed, please fix this and push again${NC}"
    echo ""
    exit $rc
fi

# ------------------------------
# run all the tests
# ------------------------------
echo ""
echo -e "${YELLOW}#####################${NC}"
echo -e "${YELLOW}  Running ALL tests  ${NC}"
echo -e "${YELLOW}#####################${NC}"
echo ""
dotnet test 
rc=$?
# $? is a shell variable which stores the return code from what we just ran
if [[ $rc != 0 ]] ; then
    # A non-zero return code means an error occurred, so tell the user and exit
    echo -e "${RED}UNIT Tests failed, please fix and push again${NC}"
    echo ""
    exit $rc
fi

# All is well
exit 0