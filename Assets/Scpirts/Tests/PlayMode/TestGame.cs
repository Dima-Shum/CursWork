using NUnit.Framework;

public class SimpleGameTests
{
    // 1. Проверка сложения
    [Test]
    public void Test_AdditionWorks()
    {
        int result = 2 + 2;
        Assert.AreEqual(4, result);
    }

    // 2. Проверка вычитания (для расчёта здоровья)
    [Test]
    public void Test_SubtractionWorks()
    {
        int health = 10;
        int damage = 3;
        int newHealth = health - damage;

        Assert.AreEqual(7, newHealth);
    }

    // 3. Проверка деления (для процента здоровья)
    [Test]
    public void Test_DivisionWorks()
    {
        float currentHealth = 7f;
        float maxHealth = 10f;
        float percent = currentHealth / maxHealth;

        Assert.AreEqual(0.7f, percent);
    }

    // 4. Проверка инкремента (для счётчика убийств)
    [Test]
    public void Test_IncrementWorks()
    {
        int kills = 0;
        kills++;
        Assert.AreEqual(1, kills);

        kills++;
        Assert.AreEqual(2, kills);
    }

    // 5. Проверка сравнения чисел (для определения жизни/смерти)
    [Test]
    public void Test_ComparisonWorks()
    {
        int health = 0;
        bool isDead = (health <= 0);
        Assert.IsTrue(isDead);

        health = 5;
        isDead = (health <= 0);
        Assert.IsFalse(isDead);
    }

    // 6. Проверка строк (для имени игрока)
    [Test]
    public void Test_StringIsNotEmpty()
    {
        string playerName = "Player123";
        Assert.IsNotEmpty(playerName);
        Assert.IsNotNull(playerName);
    }

    // 7. Проверка булевой логики (для проверки условий)
    [Test]
    public void Test_BooleanLogicWorks()
    {
        bool isRunning = true;
        bool isAlive = true;
        bool canAttack = isRunning && isAlive;

        Assert.IsTrue(canAttack);

        isAlive = false;
        canAttack = isRunning && isAlive;
        Assert.IsFalse(canAttack);
    }
}