#include <iostream>
#include <sstream>
#include <vector>
#include <algorithm>
#include <numeric>
#include "fizzbuzz.h";

int main()
{
	/* Basic version
	bool fb = false;
	for (int i = 1; i <= 100; i++)
	{
	fb = false;
	if (i % 3 == 0)
	{
	std::cout << "Fizz";
	fb = true;
	}
	if (i % 5 == 0)
	{
	std::cout << "Buzz";
	fb = true;
	}
	if (!fb)
	{
	std::cout << i;
	}
	std::cout << std::endl;
	}
	*/

	// Over engineered version
	FizzBuzz& fb = FizzBuzz{ 3, 5 };
	std::vector<int> numbers(100);
	std::iota(std::begin(numbers), std::end(numbers), 1);

	// Pick a for loop...
	/*
	for each (int i in numbers)
	{
		std::cout << fb.GetMessage(i) << std::endl;
	}
	
	// or
	
	for (auto i : numbers)
	{
		std::cout << fb.GetMessage(i) << std::endl;
	}

	// or

	std::for_each(numbers.begin(), numbers.end(), [&fb](int &n) { std::cout << fb.GetMessage(n) << std::endl; });

	*/

	for (auto i : numbers)
	{
		std::cout << fb.GetMessage(i) << std::endl;
	}

	// Wait for any key to close the window
	// eg:
	//   system("pause");
	// or
	//   std::cin.ignore();
	// or
	//   std::cin.get();
	// or
	//   _getch() in conio.h

	std::cin.ignore();
}

