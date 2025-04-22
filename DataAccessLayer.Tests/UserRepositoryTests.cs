using DataAccessLayer.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task GetUsersAsync_ReturnsAllUsersInOrder()
        {
            // Arrange
            using var ctx = InMemoryDbContextFactory.Create();
            ctx.Users.AddRange(
                new User { Id = 42, Email = "z@z", Password = "p", RoleId = 1, Roles = new Role { Id = 1, Name = "Admin" } },
                new User { Id = 10, Email = "a@a", Password = "p", RoleId = 2, Roles = new Role { Id = 2, Name = "User" } }
            );
            await ctx.SaveChangesAsync();

            var repo = new UserRepository(ctx);

            // Act
            var users = await repo.GetUsersAsync();

            // Assert
            Assert.Equal(2, users.Count);
            Assert.Equal(10, users[0].Id);
            Assert.Equal("a@a", users[0].Email);
            Assert.Equal(42, users[1].Id);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUserInOrder()
        {
            // Arrange
            using var ctx = InMemoryDbContextFactory.Create();
            ctx.Users.AddRange(
                new User { Id = 42, Email = "z@z", Password = "p", RoleId = 1, Roles = new Role { Id = 1, Name = "Admin" } },
                new User { Id = 10, Email = "a@a", Password = "p", RoleId = 2, Roles = new Role { Id = 2, Name = "User" } }
            );
            await ctx.SaveChangesAsync();

            var repo = new UserRepository(ctx);

            // Act
            var user = await repo.GetUserByIdAsync(10);

            // Assert           
            Assert.Equal(10, user.Id);
            Assert.Equal("a@a", user.Email);
            
        }

        [Fact]
        public async Task AddUserAsync_CallsAddAndSaveChanges()
        {
            // Arrange
            using var ctx = InMemoryDbContextFactory.Create();
            var repo = new UserRepository(ctx);
            var newUser = new User { Email = "new@user", Password = "pwd", RoleId = 5 };

            // Act
            var result = await repo.AddUserAsync(newUser);

            // Assert
            Assert.True(result);
            var usersInDb = await ctx.Users.ToListAsync();
            Assert.Single(usersInDb);
            Assert.Equal("new@user", usersInDb[0].Email);
        }

        [Fact]
        public async Task UpdateUserAsync_ExistingUser_UpdatesAndReturnsTrue()
        {
            // Arrange
            using var ctx = InMemoryDbContextFactory.Create();
            var user = new User { Email = "old@user", Password = "oldpwd", RoleId = 1 };
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var repo = new UserRepository(ctx);
            var updatedUser = new User { Email = "new@user", Password = "newpwd", RoleId = 2 };

            // Act
            var result = await repo.UpdateUserAsync(user.Id, updatedUser);

            // Assert
            Assert.True(result);
            var dbUser = await ctx.Users.FindAsync(user.Id);
            Assert.Equal("new@user", dbUser.Email);
            Assert.Equal("newpwd", dbUser.Password);
            Assert.Equal(2, dbUser.RoleId);
        }

        [Fact]
        public async Task UpdateUserAsync_NonExistingUser_ThrowsException()
        {
            // Arrange
            using var ctx = InMemoryDbContextFactory.Create();
            var repo = new UserRepository(ctx);
            var updatedUser = new User { Email = "x@x", Password = "pwd", RoleId = 1 };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(
                () => repo.UpdateUserAsync(42, updatedUser));
            Assert.Contains(
                "An error occurred while updating the user: User with ID 42 not found.",
                ex.Message);
        }

        [Fact]
        public async Task DeleteUserAsync_ExistingUser_RemovesAndReturnsTrue()
        {
            // Arrange
            using var ctx = InMemoryDbContextFactory.Create();
            var user = new User { Email = "todelete@user", Password = "pwd", RoleId = 1 };
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var repo = new UserRepository(ctx);

            // Act
            var result = await repo.DeleteUserAsync(user.Id);

            // Assert
            Assert.True(result);
            var exists = await ctx.Users.FindAsync(user.Id);
            Assert.Null(exists);
        }

        [Fact]
        public async Task DeleteUserAsync_NonExistingUser_ThrowsException()
        {
            // Arrange
            using var ctx = InMemoryDbContextFactory.Create();
            var repo = new UserRepository(ctx);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(
                () => repo.DeleteUserAsync(100));
            Assert.Contains(
                "An error occurred while deleting the user: User with ID 100 not found.",
                ex.Message);
        }
    }
}
