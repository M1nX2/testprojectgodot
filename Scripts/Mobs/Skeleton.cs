using Godot;
using System;

public partial class Skeleton : CharacterBody2D
{
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    bool chase = false;
    bool alive = true;
    double speed = 100;

    private AnimatedSprite2D anim;

    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = base.Velocity;

        // Add the gravity.
        if (!base.IsOnFloor())
        {
            velocity += base.GetGravity() * (float)delta;
        }
        Player player = GetNode<Player>("/root/Level/Player/Player");
        Vector2 direction = (player.Position - this.Position).Normalized();
        if (alive)
        {
            if (chase)
            {
                velocity.X = direction.X * (float)speed;
                if (direction.X < 0)
                {
                    anim.FlipH = true;
                }
                else if (direction.X > 0)
                {
                    anim.FlipH = false;
                }
                anim.Play("Run");
            }
            else
            {
                velocity.X = 0;
                anim.Play("Idle");
            }
        }

        base.Velocity = velocity;
        base.MoveAndSlide();
    }

    public void _on_detector_body_entered(Node2D body)
    {
        if (body.Name.Equals("Player"))
        {
            chase = true;
        }
    }

    public void _on_detector_body_exited(Node2D body)
    {
        if (body.Name.Equals("Player"))
        {
            chase = false;
        }
    }

    public void _on_death_body_entered(Node2D body)
    {
        if (body.Name.Equals("Player") && alive)
        {
            Player player = (Player)body;
            Vector2 velocity = player.Velocity;
            velocity.Y -= 200;
            player.Velocity = velocity;
            death();
        }
    }

    public void _on_death_2_body_entered(Node2D body)
    {
        if (body.Name.Equals("Player") && alive)
        {
            Player player = (Player)body;
            player.health -= 40;
            death();
        }
    }

    private void death()
    {
        alive = false;
        anim.Play("Death");
        anim.AnimationFinished += () => QueueFree();
    }
}
