using System;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Размеры поля и положение участников на экране.
const int fieldWidth = 7;
const int fieldHeight = 5;
const int heroX = 1;
const int heroY = 2;
const int monsterX = 5;
const int monsterY = 2;

// Параметры боя: броня уменьшает урон, но не ниже минимального.
const int maxHeroHealth = 100;
const int initialMonsterHealth = 60;
const int heroAttack = 20;
const int monsterAttack = 15;
const int heroArmor = 5;
const int minimumDamage = 1;
const int minimumHealth = 0;
const int healingAmount = 25;
const int initialPotionCount = 2;

const int exitCommand = 0;
const int attackCommand = 1;
const int healCommand = 2;

string heroName = "Искатель";
int heroHealth = maxHeroHealth;
int monsterHealth = initialMonsterHealth;
int potionCount = initialPotionCount;
bool isPlaying = true;

Console.WriteLine("ПОДЗЕМЕЛЬЕ — прототип v0.1");

while (isPlaying && heroHealth > minimumHealth
                 && monsterHealth > minimumHealth)
{
    Console.WriteLine();

    for (int y = 0; y < fieldHeight; y++)
    {
        for (int x = 0; x < fieldWidth; x++)
        {
            bool isBorder = x == 0 || x == fieldWidth - 1
                         || y == 0 || y == fieldHeight - 1;

            if (isBorder)
                Console.Write("# ");
            else if (x == heroX && y == heroY)
                Console.Write("@ ");
            else if (x == monsterX && y == monsterY)
                Console.Write("M ");
            else
                Console.Write(". ");
        }

        Console.WriteLine();
    }

    Console.WriteLine("@ — герой, M — монстр, # — стена");
    Console.WriteLine(
        $"{heroName} | Здоровье: {heroHealth}/{maxHeroHealth}" +
        $" | Броня: {heroArmor} | Зелья: {potionCount}");
    Console.WriteLine($"Здоровье монстра: {monsterHealth}");
    Console.WriteLine("1 — атаковать | 2 — выпить зелье | 0 — выйти");
    Console.Write("Команда: ");

    string? playerInput = Console.ReadLine();

    // Завершение потока ввода обрабатывается как выход из игры.
    if (playerInput is null)
    {
        isPlaying = false;
        continue;
    }

    if (!int.TryParse(playerInput, out int command))
    {
        Console.WriteLine("Ошибка: введи номер команды. Ход не потрачен.");
        continue;
    }

    switch (command)
    {
        case exitCommand:
            isPlaying = false;
            break;

        case attackCommand:
            monsterHealth = Math.Max(
                minimumHealth, monsterHealth - heroAttack);

            Console.WriteLine($"Ты наносишь {heroAttack} урона.");

            // Побеждённый монстр не может атаковать в ответ.
            if (monsterHealth > minimumHealth)
            {
                int receivedDamage = Math.Max(
                    minimumDamage, monsterAttack - heroArmor);

                heroHealth = Math.Max(
                    minimumHealth, heroHealth - receivedDamage);

                Console.WriteLine(
                    $"Монстр отвечает: {monsterAttack} − {heroArmor}" +
                    $" = {receivedDamage} урона.");
            }

            break;

        case healCommand:
            if (potionCount <= 0)
            {
                Console.WriteLine("Зелья закончились.");
                break;
            }

            if (heroHealth == maxHeroHealth)
            {
                Console.WriteLine("Здоровье уже полное. Зелье сохранено.");
                break;
            }

            // Лечение не может поднять здоровье выше максимума.
            int restoredHealth = Math.Min(
                healingAmount, maxHeroHealth - heroHealth);

            heroHealth += restoredHealth;
            potionCount--;

            Console.WriteLine($"Восстановлено здоровья: {restoredHealth}.");
            break;

        default:
            Console.WriteLine("Нет такой команды. Используй 0, 1 или 2.");
            break;
    }
}

Console.WriteLine();

if (monsterHealth <= minimumHealth)
    Console.WriteLine("Победа! Монстр повержен.");
else if (heroHealth <= minimumHealth)
    Console.WriteLine("Поражение. Герой погиб.");
else
    Console.WriteLine("Выход из игры.");

Console.WriteLine(
    $"Итог: здоровье героя — {heroHealth}, монстра — {monsterHealth}.");