#!/bin/bash
dotnet restore
dotnet list package --vulnerable --include-transitive | tee results.log
GREP_VULN=`grep -c 'has the following vulnerable packages' results.log` || true
GREP_CRIT=`grep -c 'Critical' results.log` || true
GREP_HIGH=`grep -c 'High' results.log` || true

if [[ "$GREP_VULN" != "0" ]]; then
    if [ "$GREP_CRIT" == "0" -a "$GREP_HIGH" == "0" ]; then
        echo "### Vulnerable packages found - not high or critical ###"
        exit 0
    fi
    echo "### High/critical vulnerable packages found ###"
    exit 1
fi
echo "### No vulnerable packages found ###"
exit 0