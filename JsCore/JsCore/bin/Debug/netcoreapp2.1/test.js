
function fibonacci(n)
{
    if (n == 0 || n == 1) return 1;

    var array = [1, 1];

    for (var i = 2; i < n; i++)
    {
        array.push(array[0] + array[1]);
        array.shift();
    }

    return array[0] + array[1];
}

for (var i = 0; i < 1000; i++) {
    alert(fibonacci(i));
}
