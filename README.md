# \# OMEGA Sudoku Solver Ω

\## Author: Hadar Shapira

\### This project implements a generic sudoku solver in C# as an assignment for the Omega pre-course.



\# Usage:

\## To use this solver, you must have a runtime environment installed(VS), run the Main() function. using the CLI, insert a valid string(81 digits long, with no conflicting digits) representing the board you want to solve(0 for empty cell). the output will be a visual representation of the solved board along with the corresponding formatted string.



\# Generalization:

\## The project was written with expansion options in mind - it can solve boards of size up to 64\*64, tho such boards might take much longer to solve. the current release does not allow the user to enter a board bigger than 9\*9 but changing this is very easy.



\# Testing: (AAA)

\## The project includes testing of 100000 sudoku boards, testing for fast calculation time and validity.



\# Time constraint:

\## The project was written with time constraint in mind - ensuring solving time of (much) less than 1 second for any 9\*9 board. (assuming a computer with descent computing capabilities).

