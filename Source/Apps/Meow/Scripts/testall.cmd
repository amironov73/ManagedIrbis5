@echo off

for %%q in (*.meow) do (
echo ======================
echo %%q
..\Meow %%q
echo ======================
)
